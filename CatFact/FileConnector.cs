using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CatFact
{
    class FileConnector : IFileConnector
    {
        private static readonly ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        string _filename;

        private readonly ILogger<FileConnector> _logger;

        public FileConnector(ILogger<FileConnector> logger)
        {
            _filename = Program.CatFactFilePath;
            _logger = logger;
        }
        public async Task SaveToFileAsync(string model)
        {
            lock_.EnterWriteLock();
            try
            {
                _logger.LogInformation("Saving CatFact to File...");
                using StreamWriter file = new StreamWriter(_filename, append: true);
                await file.WriteLineAsync(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"Saving Failed: {e.Message}");
            }
            finally
            {
                lock_.ExitWriteLock();
                _logger.LogInformation("CatFact saved.");
            }

        }

    }
}
