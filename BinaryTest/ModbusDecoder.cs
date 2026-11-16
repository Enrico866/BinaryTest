using System;

public class ModbusDecoder
{
    public static long DecodeRegistersToLong(ushort highWord, ushort lowWord, bool wordSwap = false)
    {
        // Se il dispositivo inverte l'ordine dei registri (Word Swap)
        if (wordSwap)
        {
            ushort temp = highWord;
            highWord = lowWord;
            lowWord = temp;
        }

        // Ricostruiamo l'array di 4 byte dai due registri da 16-bit
        byte[] bytes = new byte[4];
        
        bytes[0] = (byte)(highWord >> 8);
        bytes[1] = (byte)(highWord & 0xFF);
        bytes[2] = (byte)(lowWord >> 8);
        bytes[3] = (byte)(lowWord & 0xFF);

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        return BitConverter.ToInt32(bytes, 0);
    }
}