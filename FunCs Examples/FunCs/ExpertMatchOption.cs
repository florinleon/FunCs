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
    /// The class that implements pattern matching on options.
    /// </summary>
    public class ExpertMatchF<T>
    {
        private OptionF<T> _option;

        /// <summary>
        /// Initializes a new instance of the ExpertMatchF class.
        /// </summary>
        /// <param name="option">An option object that will be used for pattern matching.</param>
        public ExpertMatchF(OptionF<T> option)
        {
            _option = option;
        }

        /// <summary>
        /// Returns true if the option contains a value and false otherwise.
        /// </summary>
        /// <param name="some">The value of the option.</param>
        public bool MatchSome(out T some)
        {
            if (_option.IsSome)
            {
                some = _option.Value;
                return true;
            }
            else
            {
                some = default(T);
                return false;
            }
        }

        /// <summary>
        /// Returns true if the option does not contain a value and false otherwise.
        /// </summary>
        public bool MatchNone()
        {
            return _option.IsNone;
        }
    }
}