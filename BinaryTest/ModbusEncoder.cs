using System;

public class ModbusEncoder
{
    public static ushort[] EncodeLongToRegisters(long value)
    {
        // Supponiamo di voler convertire un long (o int a 32 bit) in 2 registri Modbus da 16-bit
        int intVal = (int)value; 
        
        byte[] bytes = BitConverter.GetBytes(intVal);

        // Se la macchina è Little-Endian, invertiamo per mandarla in Big-Endian standard Modbus
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        // Estraiamo le due word da 16-bit
        ushort highWord = (ushort)((bytes[0] << 8) | bytes[1]);
        ushort lowWord  = (ushort)((bytes[2] << 8) | bytes[3]);

        // Restituisce l'array di due registri pronti per la risposta Modbus (es. Holding/Input Registers)
        return new ushort[] { highWord, lowWord };
    }
}