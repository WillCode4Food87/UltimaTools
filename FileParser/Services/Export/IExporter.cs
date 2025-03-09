﻿namespace FileParser.Services.Export
{
    public interface IExporter<T>
    {
        void Export(List<T> data, string outputPath);
    }
}
