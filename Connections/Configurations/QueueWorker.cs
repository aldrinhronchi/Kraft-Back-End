namespace Kraft_Back_CS.Connections.Configurations
{
    /// <summary>
    /// Robo de Fila autogerenciavel
    /// </summary>
    public class QueueWorker<T>
    {
        public Boolean Finished = false;
        public List<T>? InQueue = new List<T>();

        /// <summary>
        /// Preenche a fila com os elementos, efetua a configuracao dos parametros
        /// </summary>
        public void Initialize(Int32 Slots = 1, List<T>? Elements = null)
        {
            if (Elements == null)
            {
                return;
            }
            this.InQueue = Elements;
            for (Int32 Index = 0; Index < InQueue.Count; Index++)
            {
                T Element = Elements[Index];
                if (Index == Slots)
                {
                    break;
                }

                if (Element == null)
                {
                    continue;
                }
                this.Process(Element);
            }
        }

        /// <summary>
        /// Efetua o processamento do elemento
        /// </summary>

        protected void Process(T Element)
        {
            Boolean Done = true;
            this.Step(Element, Done);
        }

        /// <summary>
        /// Gerencia a chamada do proximo elemento ou de encerrar a fila
        /// </summary>
        protected void Step(T Element, Boolean Done)
        {
            if (Done)
            {
                Int32 Index = this.InQueue.IndexOf(Element);
                if (Index == -1)
                {
                    this.InQueue.RemoveAt(Index);
                }
                // Exclui da fila ou altera a flag para Finalizado
            }
            // Busca o proximo a ser processado
            List<T> Result = new List<T>();
            this.InQueue = Result;
            if (this.InQueue.Count() == 0)
            {
                this.Finalize();
            }
        }

        /// <summary>
        /// Encerra a fila e faz trativas de finalizacao
        /// </summary>
        protected void Finalize()
        {
            // DO shenanigans
        }
    }
}