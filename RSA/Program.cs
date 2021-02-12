using System;
using System.IO;
using System.Numerics;

namespace RSA
{
    class Program
    {
        // Нахождение числа d.
        public static BigInteger GenerateD(BigInteger e, BigInteger phi)
        {
            BigInteger d = new BigInteger();

            for (BigInteger i = 1; i < phi; i = BigInteger.Add(i, 1))
            {
                d = BigInteger.DivRem(BigInteger.Add(BigInteger.Multiply(i, phi), 1), e, out BigInteger remainder);
                if (remainder == 0)
                {
                    break;
                }
            }
            return d;
        }

        static void Main(string[] args)
        {
            BigInteger p;
            BigInteger q;

            // Задаём числа p и q из файла (должны быть простыми).
            using (StreamReader fs = new StreamReader("pq.txt"))
            {
                p = BigInteger.Parse(fs.ReadLine());
                q = BigInteger.Parse(fs.ReadLine());
            }
            Console.WriteLine("p = " + p.ToString());
            Console.WriteLine("q = " + q.ToString());

            // Считаем число n.
            BigInteger n = p * q;
            Console.WriteLine("n = " + n.ToString());

            // Считаем число phi.
            BigInteger phi = (p - 1) * (q - 1);
            Console.WriteLine("phi = " + phi.ToString());

            // Проверка e на взаимную простоту с phi.
            BigInteger e;
            while (true)
            {
                Console.Write("Введите число e (1 < e < phi и взаимно простое с phi): ");
                e = BigInteger.Parse(Console.ReadLine());
                Console.WriteLine();
                if (BigInteger.GreatestCommonDivisor(e, phi) == BigInteger.One)
                {
                    break;
                }
            }
            Console.WriteLine("e = " + e.ToString());

            // Вычисляем число d.
            BigInteger d = GenerateD(e, phi);

            Console.WriteLine($"Открытый ключ: (e = {e}, n = {n})");
            Console.WriteLine($"Закрытый ключ: (d = {d}, n = {n})");

            // Шифруем и дешифруем сообщение.
            Console.Write("Введите число m (< n - 1), которое необходимо зашифровать: ");
            BigInteger m = BigInteger.Parse(Console.ReadLine());
            BigInteger cryptedM = BigInteger.ModPow(m, e, n);
            BigInteger decryptedM = BigInteger.ModPow(cryptedM, d, n);
            Console.WriteLine($"Зашифрованное сообщение: {cryptedM}");
            Console.WriteLine($"Расшифрованное сообщение: {decryptedM}");
        }
    }
}
