using Microsoft.EntityFrameworkCore;
using MinhaCantina.Biblioteca.Modelos;

namespace MinhaCantina.Servidor.Dados;

public class MinhaCantinaContexto(DbContextOptions contextoOpcoes) : DbContext(contextoOpcoes)
{
	public DbSet<Categoria> Categorias { get; set; }
	public DbSet<Produto> Produtos { get; set; }
	public DbSet<Usuario> Usuarios { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Categoria>().Property(categoria => categoria.Nome).HasColumnType("varchar").HasMaxLength(50).IsRequired(); // CREATE TABLE (nome VARCHAR(50) NOT NULL)
		modelBuilder.Entity<Categoria>().HasIndex(categoria => categoria.Nome).IsUnique(); // UNIQUE

		modelBuilder.Entity<Produto>().Property(produto => produto.Nome).HasColumnType("varchar").HasMaxLength(50).IsRequired();
		modelBuilder.Entity<Produto>().HasIndex(produto => produto.Nome).IsUnique();
		modelBuilder.Entity<Produto>().Property(produto => produto.Preco).HasColumnType("decimal(4, 2)").IsRequired();
		modelBuilder.Entity<Produto>().Property(produto => produto.Descricao).HasColumnType("varchar").HasMaxLength(50).IsRequired(false);

		// Configurando o Usuario
		// Modifica a propriedade Nome
		modelBuilder.Entity<Usuario>().Property(usuario => usuario.Nome).HasColumnType("varchar").HasMaxLength(100).IsRequired();
		// Modifica a propriedade Senha
		modelBuilder.Entity<Usuario>().Property(usuario => usuario.Senha).HasColumnType("text").IsRequired();
		// Modifica a propriedade Username
		modelBuilder.Entity<Usuario>().Property(usuario => usuario.Username).HasColumnType("varchar").HasMaxLength(16).IsRequired();
		modelBuilder.Entity<Usuario>().HasIndex(usuario => usuario.Username).IsUnique();
	}
}