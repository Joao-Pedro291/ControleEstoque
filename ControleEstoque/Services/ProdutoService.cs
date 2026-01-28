using ControleEstoque.DTOs;
using ControleEstoque.Models;
using System.Xml.Linq;

public class ProdutoService
{
    private static List<Produto> produtos = new()
    {
        new Produto { Id = 1, Nome = "Mouse", Quantidade = 10, Preco = 50 },
        new Produto { Id = 2, Nome = "Teclado", Quantidade = 5, Preco = 120 }
    };

    public List<Produto> ObterTodos() => produtos;

    public Produto? ObterPorId(int id)
        => produtos.FirstOrDefault(p => p.Id == id);

    public Produto Criar(Produto produto)
    {
        produto.Id = produtos.Max(p => p.Id) + 1;
        produtos.Add(produto);
        return produto;
    }

    public Produto? Atualizar(int id, CriarProdutoDto dto)
    {
        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return null;

        produto.Nome = dto.Nome;
        produto.Quantidade = dto.Quantidade;
        produto.Preco = dto.Preco;

        return produto;
    }

    public bool Remover(int id)
    {
        var produto = ObterPorId(id);
        if (produto == null) return false;

        produtos.Remove(produto);
        return true;
    }

    public Produto? EntradaEstoque(int id, int quantidade)
    {
        if (quantidade <= 0)
            return null;

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null)
            return null;

        produto.Quantidade += quantidade;
        return produto;
    }

    public Produto? SaidaEstoque(int id, int quantidade)
    {
        if (quantidade <= 0)
            return null;

        var produto = produtos.FirstOrDefault(p => p.Id == id);
        if (produto == null || produto.Quantidade < quantidade)
            return null;

        produto.Quantidade -= quantidade;
        return produto;
    }
}
