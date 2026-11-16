using System;
using System.IO;

namespace BinaryTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
        restart:    
            Console.WriteLine("\n========================================");
            Console.WriteLine("       TEST UTILITY MODBUS ENCODER      ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Codifica e decodifica un numero intero");
            Console.WriteLine("2. Test con Word Swap (Registri invertiti)");
            Console.WriteLine("3. Inserisci manualmente due registri (16-bit)");
            Console.WriteLine("0. Esci");
            Console.Write("\nScegli un'opzione: ");

            string choice = Console.ReadLine();

            if (choice == "0")
            {
                return;
            }
            else if (choice == "1" || choice == "2")
            {
                bool useWordSwap = (choice == "2");
                
                Console.Write("Inserisci un valore intero (es. 2580 o 70000): ");
                string val = Console.ReadLine();

                if (long.TryParse(val, out long intValue))
                {
                    // Codifica
                    ushort[] registers = ModbusEncoder.EncodeLongToRegisters(intValue);
                    Console.WriteLine($"\n--- RISULTATI CODIFICA ---");
                    Console.WriteLine($"Registri (Decimale): [{registers[0]}, {registers[1]}]");
                    Console.WriteLine($"Registri (Esadecimale): [0x{registers[0]:X4}, 0x{registers[1]:X4}]");

                    // Decodifica
                    double decodedValue = ModbusDecoder.DecodeRegistersToLong(registers[0], registers[1], wordSwap: useWordSwap);
                    Console.WriteLine($"\n--- RISULTATI DECODIFICA (WordSwap: {useWordSwap}) ---");
                    Console.WriteLine($"Valore decodificato: {decodedValue}");
                }
                else
                {
                    Console.WriteLine("Input non valido. Inserisci un numero intero.");
                }
            }
            else if (choice == "3")
            {
                Console.Write("Inserisci il registro Alto (High Word - decimale o esadecimale es. 0x0001): ");
                string highStr = Console.ReadLine();
                Console.Write("Inserisci il registro Basso (Low Word - decimale o esadecimale): ");
                string lowStr = Console.ReadLine();

                // Supporta anche la lettura in esadecimale (es. 0xA14 o valori decimali)
                if (TryParseUniversalInt(highStr, out ushort highWord) && TryParseUniversalInt(lowStr, out ushort lowWord))
                {
                    Console.WriteLine($"\nRegistri inseriti -> High: {highWord} (0x{highWord:X4}), Low: {lowWord} (0x{lowWord:X4})");

                    long normalDecode = ModbusDecoder.DecodeRegistersToLong(highWord, lowWord, wordSwap: false);
                    long swappedDecode = ModbusDecoder.DecodeRegistersToLong(highWord, lowWord, wordSwap: true);

                    Console.WriteLine($"Valore decodificato (Normale): {normalDecode}");
                    Console.WriteLine($"Valore decodificato (Con Word Swap): {swappedDecode}");
                }
                else
                {
                    Console.WriteLine("Formato dei registri non valido.");
                }
            }
            else
            {
                Console.WriteLine("Opzione non valida.");
            }

            Console.WriteLine("\nPremmi un tasto per continuare...");
            Console.ReadKey();
            Console.Clear();
            goto restart;
        }

        // Metodo di supporto per leggere sia numeri decimali che esadecimali (es. 0x0A14)
        private static bool TryParseUniversalInt(string input, out ushort result)
        {
            input = input.Trim();
            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return ushort.TryParse(input.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            return ushort.TryParse(input, out result);
        }
    }
}