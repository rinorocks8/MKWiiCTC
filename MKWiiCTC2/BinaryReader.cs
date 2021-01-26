using System.Text;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MKWiiCTC2
{
    public enum Endianness
    {
        BigEndian,
        LittleEndian,
    }

    public class EndianBinaryReader : IDisposable
    {
        private bool disposed;
        private byte[] buffer;

        public Stream BaseStream { get; private set; }
        public Endianness Endianness { get; set; }
        public Endianness SystemEndianness { get { return BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian; } }

        private bool Reverse { get { return SystemEndianness != Endianness; } }

        public EndianBinaryReader(Stream baseStream)
            : this(baseStream, Endianness.BigEndian)
        { }

        public EndianBinaryReader(Stream baseStream, Endianness endianness)
        {
            if (baseStream == null) throw new ArgumentNullException("baseStream");
            if (!baseStream.CanRead) throw new ArgumentException("baseStream");

            BaseStream = baseStream;
            Endianness = endianness;
        }

        ~EndianBinaryReader()
        {
            Dispose(false);
        }

        private void FillBuffer(int bytes, int stride)
        {
            if (buffer == null || buffer.Length < bytes)
                buffer = new byte[bytes];

            BaseStream.Read(buffer, 0, bytes);

            if (Reverse)
                for (int i = 0; i < bytes; i += stride)
                {
                    Array.Reverse(buffer, i, stride);
                }
        }

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (BaseStream != null)
                    {
                        BaseStream.Close();
                    }
                }

                buffer = null;

                disposed = true;
            }
        }

        public void offset(byte offset)
        {
            buffer = BitConverter.GetBytes(BaseStream.Seek(offset, SeekOrigin.Current));
        }
        public void offset(byte[] offsetArray)
        {
            if (offsetArray.Length == 2)
            {
                buffer = BitConverter.GetBytes(BaseStream.Seek(BitConverter.ToUInt16(offsetArray, 0), SeekOrigin.Current));
            } 
            else if (offsetArray.Length == 4)
            {
                buffer = BitConverter.GetBytes(BaseStream.Seek(BitConverter.ToUInt32(offsetArray, 0), SeekOrigin.Current));
            }
            else if (offsetArray.Length == 8)
            {
                buffer = BitConverter.GetBytes(BaseStream.Seek(BitConverter.ToInt64(offsetArray, 0), SeekOrigin.Current));
            }
        }

        public void backset(byte offset)
        {
            buffer = BitConverter.GetBytes(BaseStream.Seek(-offset, SeekOrigin.Current));
        }

        public void goTo(byte start)
        {
            BaseStream.Position = start;
        }
        public void goTo(byte[] startArray)
        {
            if (startArray.Length == 2)
            {
                BaseStream.Position = BitConverter.ToUInt16(startArray, 0);
            }
            else if (startArray.Length == 4)
            {
                BaseStream.Position = BitConverter.ToUInt32(startArray, 0);
            }
            else if (startArray.Length == 8)
            {
                BaseStream.Position = BitConverter.ToInt64(startArray, 0);
            }
        }

        public byte[] getLoc()
        {
            byte[] a = BitConverter.GetBytes(BaseStream.Position);
            return a;
        }

        public void displayLoc()
        {
            displayBytes(BitConverter.GetBytes(BaseStream.Position));
        }

        public void displayBytes(byte[] temp)
        {
            Array.Reverse(temp, 0, temp.Length);
            System.Windows.Forms.MessageBox.Show(BitConverter.ToString(temp));
        }

        public byte[] readBytes(int count)
        {

            FillBuffer(count, 1);
            byte[] temp = new byte[count];
            Array.Copy(buffer, 0, temp, 0, count);
            Array.Reverse(temp, 0, temp.Length);

            backset(Convert.ToByte(count*sizeof(byte)));

            return temp;
        }

        public string readBytesToString(int count)
        {
            byte[] temp = readBytes(count);
            Array.Reverse(temp, 0, temp.Length);
            return Encoding.UTF8.GetString(temp);
        }

        public void writeByte(byte value)
        {
            BaseStream.WriteByte(value);
            backset(sizeof(byte));
        }

        public void writeBytes(byte[] values)
        {
            BaseStream.Write(values, 0, values.Length);
            backset(Convert.ToByte(values.Length * sizeof(byte)));
        }

        public void seek(string name)
        {
            while (true)
            {
                string nextString = readBytesToString(name.Length);
                offset(sizeof(byte));
                if (nextString == name)
                {
                    backset(sizeof(byte));
                    break;
                }
            }
        }

        public string scan()
        {
            byte a = 0;
            while (true)
            {
                if (!readBytes(4).SequenceEqual(new byte[4]))
                {
                    a++;
                    offset(sizeof(byte));
                }
                else
                {
                    backset(a);
                    return readBytesToString(a);
                }
            }
        }

    }
}
 