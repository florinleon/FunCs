/**************************************************************************
 *                                                                        *
 *  This code is adapted from the following project:                      *
 *  garyriley, CLIPS Rule Based Programming Language. Expert System Tool  *
 *  https://sourceforge.net/projects/clipsrules/files/CLIPS/6.40_Beta_2/  *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CLIPSNET;

namespace RouterFormsExample
{
    internal class RouterTextBox : TextBox
    {
        private class RouterThreadBridge
        {
            public bool CharNeeded = false;
            public bool Closed = false;
            public List<Byte> CharList = new List<Byte>();
        }

        private class TextBoxRouter : Router
        {
            private delegate void AddTextCallback(string text);

            private static int _RouterTextBoxNameIndex = 0;
            private RouterTextBox _routerTextBox;
            public String RouterName;

            public TextBoxRouter(RouterTextBox theTextBox)
            {
                _routerTextBox = theTextBox;
                RouterName = "RouterTextBox" + _RouterTextBoxNameIndex++;
            }

            public override bool Query(String logicalName)
            {
                if (logicalName.Equals(CLIPSNET.Router.STANDARD_OUTPUT) ||
                    logicalName.Equals(CLIPSNET.Router.STANDARD_INPUT))
                    return true;
                else
                    return false;
            }

            public void AddText(string text)
            {
                if (_routerTextBox.InvokeRequired)
                {
                    Form parentForm = _routerTextBox.FindForm();
                    AddTextCallback d = new AddTextCallback(AddText);
                    parentForm.Invoke(d, new object[] { text });
                }
                else
                {
                    _routerTextBox.AppendText(text);
                }
            }

            public override void Print(String logicalName, String printString)
            {
                this.AddText(printString);
            }

            public override int Getc(String logicalName)
            {
                RouterThreadBridge theBridge = _routerTextBox._threadBridge;

                lock (theBridge)
                {
                    if (theBridge.Closed)
                    {
                        _routerTextBox._attachedEnv.SetHaltExecution(true);
                        return -1;
                    }

                    if (theBridge.CharList.Count == 0)
                    {
                        theBridge.CharNeeded = true;

                        try
                        {
                            Monitor.Wait(theBridge);
                        }
                        catch (SynchronizationLockException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch (ThreadInterruptedException e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    theBridge.CharNeeded = false;
                    if (theBridge.Closed)
                    {
                        _routerTextBox._attachedEnv.SetHaltExecution(true);
                        return -1;
                    }

                    Byte theByte = theBridge.CharList[0];
                    theBridge.CharList.RemoveAt(0);

                    return theByte;
                }
            }

            public override int Ungetc(String logicalName, int theChar)
            {
                lock (_routerTextBox._threadBridge)
                {
                    _routerTextBox._threadBridge.CharList.Insert(0, (Byte)theChar);
                }
                return 0;
            }
        }

        private CLIPSNET.Environment _attachedEnv;
        private TextBoxRouter _textBoxRouter;
        private RouterThreadBridge _threadBridge;

        public RouterTextBox() : base()
        {
            _textBoxRouter = new TextBoxRouter(this);
            _threadBridge = new RouterThreadBridge();
            this.AcceptsReturn = true;
            this.ReadOnly = true;
            this.Multiline = true;
        }

        public void AttachRouter(CLIPSNET.Environment theEnv, int priority)
        {
            _attachedEnv = theEnv;
            theEnv.AddRouter(_textBoxRouter.RouterName, priority, _textBoxRouter);
        }

        public void DetachRouter()
        {
            this.ReadOnly = true;
        }

        public void OnClosing()
        {
            lock (_threadBridge)
            {
                _threadBridge.Closed = true;
                if (_threadBridge.CharNeeded)
                {
                    _threadBridge.CharNeeded = false;
                    Monitor.Pulse(_threadBridge);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            lock (_threadBridge)
            {
                if (_threadBridge.CharNeeded)
                {
                    if (((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back)) && (_attachedEnv.InputBufferCount() == 0))
                    {
                        // Ignore
                    }
                    else
                    {
                        this.ReadOnly = false;
                    }
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            this.ReadOnly = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            lock (_threadBridge)
            {
                if (_threadBridge.CharNeeded)
                {
                    _threadBridge.CharList.AddRange(Encoding.UTF8.GetBytes(e.KeyChar.ToString()));
                    this.Select(this.TextLength, this.TextLength);
                    this.ScrollToCaret();
                    base.OnKeyPress(e);
                    _threadBridge.CharNeeded = false;
                    Monitor.Pulse(_threadBridge);
                }
            }
        }
    }
}