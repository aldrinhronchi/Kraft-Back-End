using Kraft_Back_CS.Models.Usuario;
using Kraft_Back_CS.Models.ViewModels;

namespace Kraft_Back_CS.Services.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        RequisicaoViewModel<Usuario> Listar(Int32 Pagina, Int32 RegistrosPorPagina, String CamposQuery = "", String ValoresQuery = "", String Ordenacao = "", Boolean Ordem = false);

        Boolean Salvar(Usuario userViewModel);

        Boolean Excluir(String ID);

        RequisicaoViewModel<Usuario> Autenticar(LoginViewModel Requisicao);
    }
}