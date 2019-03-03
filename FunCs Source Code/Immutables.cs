/**************************************************************************
 *                                                                        *
 *  Description: FunCs Functional Programming Library                     *
 *  Website:     https://github.com/florinleon/FunCs                      *
 *  Copyright:   (c) 2019, Florin Leon                                    *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

namespace FunCs
{
    /// <summary>
    /// Represents an immutable 32-bit signed integer.
    /// </summary>
    public struct IntF
    {
        /// <summary>
        /// The read-only int value.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IntF structure.
        /// </summary>
        /// <param name="value">The integer value used for initialization</param>
        public IntF(int value)
        {
            Value = value;
        }

        /// <summary>
        /// IntF can be implicitly converted to int so that it can be used for all the operations defined for int.
        /// </summary>
        public static implicit operator int(IntF i)
        {
            return i.Value;
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        override public string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// Represents an immutable double-precision floating-point number.
    /// </summary>
    public struct DoubleF
    {
        /// <summary>
        /// The read-only double value.
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DoubleF structure.
        /// </summary>
        /// <param name="value">The real value used for initialization</param>
        public DoubleF(double value)
        {
            Value = value;
        }

        /// <summary>
        /// DoubleF can be implicitly converted to double so that it can be used for all the operations defined for double.
        /// </summary>
        public static implicit operator double(DoubleF d)
        {
            return d.Value;
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        override public string ToString()
        {
            return Value.ToString("F3");
        }
    }
}