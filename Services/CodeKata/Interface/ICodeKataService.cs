using System.Text.RegularExpressions;

namespace Kraft_Back_CS.Services.CodeKata.Interface
{
    public interface ICodeKataService
    {
        public string FindNeedle(object[] haystack);

        /// <summary>
        /// Even Or Odd? - Par ou Impar?
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string EvenOrOdd(int number);

        /// <summary>
        /// Is this my tail? - Esta é minha calda?
        /// </summary>
        /// <param name="body"></param>
        /// <param name="tail"></param>
        /// <returns></returns>
        public bool CorrectTail(string body, string tail);

        /// <summary>
        /// Strip Comments - Arrancar Comentarios
        /// </summary>
        /// <param name="text"></param>
        /// <param name="commentSymbols"></param>
        /// <returns></returns>
        public string StripComments(string text, string[] commentSymbols);

        /// <summary>
        /// Double Char - Caracteres Duplos
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string DoubleChar(string s);

        /// <summary>
        /// String repeat - Repetir Texto
        /// </summary>
        /// <param name="n">Quantas vezes quer repetir</param>
        /// <param name="s">Texto a se repetir</param>
        /// <returns></returns>

        public string RepeatStr(int n, string s);

        /// <summary>
        /// Will there be enough space? - Vai ter espaço?
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="on"></param>
        /// <param name="wait"></param>
        /// <returns></returns>
        public int Enough(int cap, int on, int wait);

        /// <summary>
        /// Keep up the hoop - Contador de "pulos"
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>

        public string HoopCount(int n);

        /// <summary>
        /// Speed Control
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <returns></returns>

        public int SpeedControl(int s, double[] x);

        // Sum Arrays
        public double SumArray(double[] array);

        // Find the smallest integer in the array
        public int FindSmallestInt(int[] args);
    }
}