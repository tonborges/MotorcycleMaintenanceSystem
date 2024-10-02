#!/bin/bash
 
# Mostra o uso do script se não houver argumentos
if [ -z "$1" ]; then
    echo "Uso: $0 <command> [name]"
    echo "Comandos disponíveis:"
    echo "  add     - Adiciona uma nova migration com o nome fornecido"
    echo "  update  - Atualiza o banco de dados"
    echo "  rem     - Remove a migration anterior"
    echo "  script  - Gera o script SQL das migrações"
    echo "  list    - Lista as migrations"
    exit 1
fi
 
# Define os diretórios do projeto
PROJECT_DIR="./src/Infrastructure/"
STARTUP_PROJECT_DIR="./src/Web.Api/"
 
# Define o contexto
CONTEXT="ApplicationDbContext"
 
# Executa ações baseadas no comando
case "$1" in
    add)
        if [ -z "$2" ]; then
            echo "Por favor, forneça um nome para a migração."
            exit 1
        fi
        echo "Adicionando migração $2..."
        dotnet ef migrations add $2 --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR
        ;;
    update)
        echo "Atualizando o banco de dados..."
        if [ -z "$2" ]; then
            echo "Atualizando o banco de dados para a última migration aplicada..."
            dotnet ef database update --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR
        else
            echo "Revertendo para a migration: $2"
            dotnet ef database update $2 --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR
        fi
        ;;
    script)
        echo "Gerando script SQL..."
        dotnet ef migrations script --idempotent --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR --output script.sql
        ;;
    rem)
        echo "Removendo a última migration..."
        dotnet ef migrations remove --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR
        ;;
    list)
        echo "Listando as migrations..."
        dotnet ef migrations list --context $CONTEXT --project $PROJECT_DIR --startup-project $STARTUP_PROJECT_DIR
        ;;
    *)
        echo "Comando inválido: $1"
        echo "Use 'add', 'update', 'rem', 'script' ou 'list' como primeiro argumento."
        exit 1
        ;;
esac