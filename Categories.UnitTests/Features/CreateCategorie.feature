# language: pt-BR
Funcionalidade: Gestão de Categorias
    Como um administrador do sistema
    Eu quero cadastrar novas categorias de produtos
    Para que eu possa organizar o cardápio

Cenario: Criar uma categoria comum com sucesso
    Dado que eu quero criar uma categoria com nome "Bebidas" e editável
    Quando eu solicito a criação desta categoria
    Entao a categoria deve ser salva no repositório
    E a categoria não deve ser considerada um Lanche
    E a data de criação deve ser recente

Cenario: Criar uma categoria do tipo Lanche
    Dado que eu quero criar uma categoria com nome "Lanche" e editável
    Quando eu solicito a criação desta categoria
    Entao a categoria deve ser identificada como Lanche