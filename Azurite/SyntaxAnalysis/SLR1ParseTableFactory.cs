
using Azurite.SyntaxAnalysis.Grammar;
using Azurite.SyntaxAnalysis.SyntaxParsingTable;
using Azurite.SyntaxAnalysis.SyntaxTree;
using System.Collections.Generic;

namespace Azurite.SyntaxAnalysis
{

    /// <summary>
    /// Factory class to create SLR1 parsing tables.
    /// </summary>
    public static class SLR1ParseTableFactory
    {

        /// <summary>
        /// Builds a parse table based on the grammar provided.
        /// </summary>
        /// <param name="grammar">The grammar</param>
        /// <returns>A parsing table</returns>
        public static ParsingTable BuildParseTable(SyntaxGrammar grammar, List<List<SLR1Item>> config, List<TerminalList> followSet)
        {
            if (!grammar.IsValid())
            {
                throw new System.Exception("Cannot build parse table with invalid input grammar.");
            }

            ParsingTable table = new ParsingTable();

            foreach (var t in grammar.Terminals)
            {
                table.AddHeader(t);
            }
            
            var extremalToken = new SyntaxTreeTerminal(new ExtremalToken());
            table.AddHeader(extremalToken);

            foreach (var nt in grammar.Nonterminals)
            {
                table.AddHeader(nt);
            }

            for (int i = 0; i < config.Count; ++i)
            {
                table.AddRow();

                for (int j = 0; j < config[i].Count; ++j)
                {
                    if (config[i][j].State < config[i][j].Rule.RightSide.Count)
                    {
                        SyntaxTreeNonterminal nt = config[i][j].Rule.RightSide[config[i][j].State] as SyntaxTreeNonterminal;
                        SyntaxTreeTerminal t = config[i][j].Rule.RightSide[config[i][j].State] as SyntaxTreeTerminal;

                        if (null != nt)
                        {
                            int idx = table.parsingTableHeader.FindIndex(x => x.CompareTo(nt) == 0);

                            table.parsingTable[i][idx].ElementType = ParsingTableElementType.jump;
                            table.parsingTable[i][idx].Value = config[i][j].Target;
                        }
                        else if (null != t)
                        {
                            int idx = table.parsingTableHeader.FindIndex(x => x.CompareTo(t) == 0);

                            table.parsingTable[i][idx].ElementType = ParsingTableElementType.shift;
                            table.parsingTable[i][idx].Value = config[i][j].Target;
                        }
                        else
                        {
                            throw new System.Exception("Cannot build parse table based on invalid syntax tree element.");
                        }
                    }
                    else
                    {
                        if (0 == config[i][j].Rule.CompareTo(grammar.ProductionRules[0]))
                        {
                            int extremalIndex = table.parsingTableHeader.FindIndex(x => x.CompareTo(extremalToken) == 0);

                            table.parsingTable[i][extremalIndex].ElementType = ParsingTableElementType.accept;
                        }
                        else
                        {
                            var follow = followSet.Find(x => x.NonTerminal.CompareTo(config[i][j].Rule.LeftSide) == 0);

                            foreach (var t in follow.Terminals)
                            {
                                int idx = table.parsingTableHeader.FindIndex(x => x.CompareTo(t) == 0);

                                table.parsingTable[i][idx].ElementType = ParsingTableElementType.reduce;

                                var productionIdx = grammar.ProductionRules.FindIndex(x => x.CompareTo(config[i][j].Rule) == 0);

                                table.parsingTable[i][idx].Value = productionIdx;
                            }
                        }
                    }
                }
            }

            return table;
        }

    }

}
