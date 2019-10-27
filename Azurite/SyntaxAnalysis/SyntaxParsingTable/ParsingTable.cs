
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.ParseConfiguration;
using Azurite.SyntaxAnalysis.ParseSets;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis.SyntaxParsingTable
{

    /// <summary>
    /// A parsing table that is generated based on the grammar.
    /// </summary>
    public class ParsingTable
    {

        /// <summary>
        /// Header of the parsing table, it contains terminals and nonterminals
        /// and also the extermal symbol (which represent the end of file).
        /// </summary>
        public List<SyntaxTreeElement> Header { get; set; }

        /// <summary>
        /// The parsing table.
        /// </summary>
        public List<List<ParsingTableElement>> Table { get; set; }

        /// <summary>
        /// Constructor of the parsing table.
        /// </summary>
        public ParsingTable(SyntaxGrammar grammar, SLR1Configuration slr1Config, List<TerminalList> followSet)
        {
            Header = new List<SyntaxTreeElement>();

            Table = new List<List<ParsingTableElement>>();

            BuildParseTable(grammar, slr1Config, followSet);
        }

        /// <summary>
        /// Adds a header to the parsing table.
        /// </summary>
        /// <param name="terminal">Terminal or nonterminal symbol to add</param>
        public void AddHeader(SyntaxTreeElement terminal)
        {
            /* Throw exception in case the entire parsing table is already generated. */
            if (Table.Count > 0)
            {
                throw new System.Exception("Unable to modify header after a row has been added.");
            }

            Header.Add(terminal);
        }

        /// <summary>
        /// Adds a row to the parsing table.
        /// </summary>
        public void AddRow()
        {
            Table.Add(new List<ParsingTableElement>());

            for (int i = 0; i < Header.Count; ++i)
            {
                Table[Table.Count - 1].Add(new ParsingTableElement(ParsingTableElementType.error));
            }
        }

        /// <summary>
        /// Builds a parse table based on the grammar provided.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>A parsing table</returns>
        public void BuildParseTable(SyntaxGrammar grammar, SLR1Configuration slr1Config, List<TerminalList> followSet)
        {
            if (!grammar.IsValid())
            {
                throw new System.Exception("Cannot build parse table with invalid input grammar.");
            }
            
            foreach (var t in grammar.Terminals)
            {
                AddHeader(t);
            }

            var extremalToken = new SyntaxTreeTerminal(new ExtremalToken());
            AddHeader(extremalToken);

            foreach (var nt in grammar.Nonterminals)
            {
                AddHeader(nt);
            }

            for (int i = 0; i < slr1Config.Config.Count; ++i)
            {
                AddRow();

                for (int j = 0; j < slr1Config.Config[i].Count; ++j)
                {
                    if (slr1Config.Config[i][j].State < slr1Config.Config[i][j].Rule.RightSide.Count)
                    {
                        SyntaxTreeNonterminal nt = slr1Config.Config[i][j].Rule.RightSide[slr1Config.Config[i][j].State] as SyntaxTreeNonterminal;
                        SyntaxTreeTerminal t = slr1Config.Config[i][j].Rule.RightSide[slr1Config.Config[i][j].State] as SyntaxTreeTerminal;

                        if (null != nt)
                        {
                            int idx = Header.FindIndex(x => x.Equals(nt));

                            Table[i][idx].ElementType = ParsingTableElementType.jump;
                            Table[i][idx].Value = slr1Config.Config[i][j].Target;
                        }
                        else if (null != t)
                        {
                            int idx = Header.FindIndex(x => x.Equals(t));

                            Table[i][idx].ElementType = ParsingTableElementType.shift;
                            Table[i][idx].Value = slr1Config.Config[i][j].Target;
                        }
                        else
                        {
                            throw new System.Exception("Cannot build parse table based on invalid syntax tree element.");
                        }
                    }
                    else
                    {
                        if (slr1Config.Config[i][j].Rule.Equals(grammar.ProductionRules[0]))
                        {
                            int extremalIndex = Header.FindIndex(x => x.Equals(extremalToken));

                            Table[i][extremalIndex].ElementType = ParsingTableElementType.accept;
                        }
                        else
                        {
                            var follow = followSet.Find(x => x.NonTerminal.Equals(slr1Config.Config[i][j].Rule.LeftSide));

                            foreach (var t in follow.Terminals)
                            {
                                int idx = Header.FindIndex(x => x.Equals(t));

                                Table[i][idx].ElementType = ParsingTableElementType.reduce;

                                var productionIdx = grammar.ProductionRules.FindIndex(x => x.Equals(slr1Config.Config[i][j].Rule));

                                Table[i][idx].Value = productionIdx;
                            }
                        }
                    }
                }
            }
        }

    }
}
