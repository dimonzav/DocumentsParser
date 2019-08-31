﻿namespace DocumentsParser
{
    using CommandLine;

    public class ParserOptions
    {
        [Option('f', "folder", Required = true, HelpText = "Please, specify the path to folder with log files.")]
        public string LogsFolderPath { get; set; }
    }

    [Verb("a", HelpText = "Run document parser using Parallel.ForEach method.")]
    public class ParallelForeachParserOptions : ParserOptions
    { }

    [Verb("b", HelpText = "Run document parser using BlockingCollection with running two parallel tasks.")]
    public class BlockingCollectionParserOptions : ParserOptions
    { }
}