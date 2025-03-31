using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Kraft_Back_CS.Models.Entities;
using Kraft_Back_CS.Extensions.Helpers;
using Kraft_Back_CS.Connections.Database;

namespace Kraft_Back_CS.Extensions.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public ErrorMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseContext database)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception exception)
            {
                ErrorInfo errorInfo = await FormatExceptionAsync(exception);
                context.Response.StatusCode = errorInfo.StatusCode;
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = null
                };

                await context.Response.WriteAsJsonAsync(errorInfo, options);
            }
        }

        public static async Task<ErrorInfo> FormatExceptionAsync(Exception exception)
        {
            ErrorInfo errorInfo = new ErrorInfo();

            switch (exception)
            {
                case KeyNotFoundException ex:
                    {
                        errorInfo.StatusCode = (int)HttpStatusCode.NotFound;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                case ValidationException ex:
                    {
                        errorInfo.StatusCode = (int)HttpStatusCode.BadRequest;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                case UnauthorizedAccessException ex:
                    {
                        errorInfo.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                default:
                    {
                        errorInfo.StatusCode = (int)HttpStatusCode.InternalServerError;
                        try
                        {
                            string[] Aplicacao = SearchMethodNameAndClass(exception);

                            Erro Falha = new Erro()
                            {
                                Aplicacao = Aplicacao.Count() == 2 ? $"{Aplicacao[0]} | {Aplicacao[1]}" : "Sotequi | Geral",
                                Data = DateTime.Now,
                                Tipo = exception.GetType().ToString(),
                                Nome = exception.TargetSite != null ? exception.TargetSite.Name : string.Empty,
                                Mensagem = exception.Message,
                                Stack = !string.IsNullOrWhiteSpace(exception.StackTrace) ? exception.StackTrace : string.Empty,
                                Arquivo = exception.Source?.ToString(),
                            };
                            using (DatabaseContext database = new DatabaseContext())
                            {
                                Erro? Error = database.ErrosLog.FirstOrDefault(x => x.Aplicacao == Falha.Aplicacao
                                                                           && x.Mensagem == Falha.Mensagem
                                                                           && x.Nome == x.Nome &&
                                                                           x.Data.Day == DateTime.Now.Day && x.Data.Month == DateTime.Now.Month && x.Data.Year == DateTime.Now.Year);
                                if (Error != null)
                                {
                                    await database.OcorrenciaLog.AddAsync(new Ocorrencia()
                                    {
                                        Aplicacao = Falha.Aplicacao,
                                        Data = Falha.Data,
                                        IDErro = Error.ID
                                    });
                                    Falha = Error;
                                }
                                else
                                {
                                    await database.ErrosLog.AddAsync(Falha);
                                }
                                await database.SaveChangesAsync();
                            }

                            errorInfo.Message = $"Houve um erro, Protocolo: {Falha.ID},\n Mensagem: {exception.Message}";
                        }
                        catch (Exception ex)
                        {
                            DebugInFile(ex.Message, "ErrorForLog");
                            errorInfo.Message = $"Houve um erro, Protocolo: 0,\n Mensagem: {exception.Message} \n Favor Contactar o suporte.";
                        }
                    }
                    break;
            }
            return errorInfo;
        }

        private static string[] SearchMethodNameAndClass(Exception ex)
        {
            if (!string.IsNullOrWhiteSpace(ex.StackTrace))
            {
                StackFrame? Frame = new StackTrace(ex).GetFrame(0);
                if (Frame != null)
                {
                    System.Reflection.MethodBase? Method = Frame.GetMethod();
                    if (Method != null)
                    {
                        string[] Array = new string[]
                        {
                            Method.GetMethodName(),
                            Method.GetClassName()
                        };
                        return Array;
                    }
                }
            }
            return new string[] { };
        }

        private static void DebugInFile(string responseMessage, string place)
        {
            using (FileStream fs = new FileStream($"{Environment.CurrentDirectory}/Files/Debug/System_Debug.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("*****************************************************");
                sw.WriteLine(place.ToString());
                sw.WriteLine("-----------------------------------------------------");
                sw.WriteLine(responseMessage.ToString());
                sw.WriteLine("=====================================================");
            }
        }
    }
}