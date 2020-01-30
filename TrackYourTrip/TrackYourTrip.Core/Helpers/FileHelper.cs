using System;
using System.IO;

namespace TrackYourTrip.Core.Helpers
{
    public class FileHelper
    {
        public bool CopyFile(string sourceFilePath, string destinationFilePath, bool overwriteIfExists)
        {
            try
            {
                if (File.Exists(destinationFilePath) && overwriteIfExists)
                {
                    File.Delete(destinationFilePath);
                }

                if (!File.Exists(destinationFilePath))
                {
                    using (BinaryReader br = new BinaryReader(File.Open(sourceFilePath, FileMode.Open, FileAccess.Read)))
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(destinationFilePath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CopyFile(Stream stream, string destinationFilePath, bool overwriteIfExists)
        {
            try
            {
                if (File.Exists(destinationFilePath) && overwriteIfExists)
                {
                    File.Delete(destinationFilePath);
                }

                if (!File.Exists(destinationFilePath))
                {
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        using (BinaryWriter bw = new BinaryWriter(new FileStream(destinationFilePath, FileMode.Create)))
                        {
                            byte[] buffer = new byte[2048];
                            int length = 0;
                            while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                bw.Write(buffer, 0, length);
                            }
                        }
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
