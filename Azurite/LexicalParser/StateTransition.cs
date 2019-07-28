
using System;
using System.Collections.Generic;

namespace Azurite.LexicalParser
{
    /// <summary>
    /// A class that stores a possible state transition.
    /// There are two possible ways to transition between
    /// two states: the first is to accept any character
    /// with a list of exceptions that are not accepted,
    /// the second is to accept a list of characters and/or
    /// a list of possible character intervals.
    /// </summary>
    public class StateTransition
    {

        /// <summary>
        /// The target state's number.
        /// </summary>
        public int Target { get; set; }

        /// <summary>
        /// This property is set to true if any character is
        /// accepted with a list of exceptions.
        /// </summary>
        public bool Any { get; private set; }

        /// <summary>
        /// The list of accepted characters.
        /// Must be ignored if any character is accepted with
        /// exceptions.
        /// </summary>
        public List<char> AcceptedChars { get; private set; }

        /// <summary>
        /// The list of accepted intervals.
        /// Must be ignored if any character is accepted with
        /// exceptions.
        /// </summary>
        public List<Tuple<char, char>> AcceptedIntervals { get; private set; }

        /// <summary>
        /// The exceptions if any character is accepted with
        /// exceptions. If this is not the case, it must be
        /// ignored.
        /// </summary>
        public List<char> Exceptions { get; private set; }

        /// <summary>
        /// The constructor of the state transition.
        /// </summary>
        public StateTransition(bool any, int target)
        {
            AcceptedChars = new List<char>();
            AcceptedIntervals = new List<Tuple<char, char>>();
            Exceptions = new List<char>();
            Target = target;
            Any = any;
        }

        /// <summary>
        /// Adds a character to the list of accepted characters.
        /// </summary>
        /// <param name="c">The character to be added</param>
        public void AddAcceptedChar(char c)
        {
            AcceptedChars.Add(c);
        }

        /// <summary>
        /// Adds an interval to the list of accepted intervals.
        /// </summary>
        /// <param name="a">The lower end of the interval</param>
        /// <param name="b">The upper end of the interval</param>
        public void AddAcceptedInterval(char a, char b)
        {
            AcceptedIntervals.Add(Tuple.Create(a, b));
        }

        public void AddException(char e)
        {
            Exceptions.Add(e);
        }

        /// <summary>
        /// Checks if the given character is accepted in this
        /// state transition.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsCharAccepted(char c)
        {
            bool retVal = false;

            /* Checks if any character is accepted with exceptions. */
            if (Any)
            {
                int ind = Exceptions.FindIndex(x => x == c);

                if (-1 == ind)
                {
                    retVal = true;
                }
            }
            else
            {
                /* Check if there is an intervals that fits the given character. */
                var it = AcceptedIntervals.Find(x => x.Item1 <= c && x.Item2 >= c);

                if (null != it)
                {
                    retVal = true;
                }
                else
                {
                    /* Check if the given character is one of the accepted characters. */
                    var it2 = AcceptedChars.Find(x => x == c);

                    if (default(char) != it2)
                    {
                        retVal = true;
                    }
                }
            }

            return retVal;
        }

    }
}
