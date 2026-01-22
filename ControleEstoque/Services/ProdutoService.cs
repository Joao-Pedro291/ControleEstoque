using ControleEstoque.Models;

namespace ControleEstoque.Services
{
    public class ProdutoService
    {
        private static List<Produto> produtos = new List<Produto>
        {
            new Produto { Id = 1, Nome = "Mouse", Quantidade = 10, Preco = 50 },
            new Produto { Id = 2, Nome = "Teclado", Quantidade = 5, Preco = 120 }
        };

        public List<Produto> ObterTodos()
        {
            return produtos;
        }

        public Produto? ObterPorId(int id)
        {
            return produtos.FirstOrDefault(p => p.Id == id);
        }

        public Produto Criar(Produto produto)
        {
            produto.Id = produtos.Count + 1;
            produtos.Add(produto);
            return produto;
        }

        public bool Remover(int id)
        {
            var produto = ObterPorId(id);
            if (produto == null) return false;

            produtos.Remove(produto);
            return true;
        }

        public bool EntradaEstoque(int id, int quantidade)
        {
            var produto = ObterPorId(id);
            if (produto == null) return false;

            produto.Quantidade += quantidade;
            return true;
        }

        public bool SaidaEstoque(int id, int quantidade)
        {
            var produto = ObterPorId(id);
            if (produto == null || produto.Quantidade < quantidade)
                return false;

            produto.Quantidade -= quantidade;
            return true;
        }
    }
}
