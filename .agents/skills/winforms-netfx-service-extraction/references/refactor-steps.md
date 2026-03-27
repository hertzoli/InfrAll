# Refactor Steps

Sequencia recomendada:

1. Identificar um unico caso de uso.
2. Isolar entradas e saidas.
3. Extrair validacao e regra pura primeiro.
4. Introduzir classe independente em arquivo proprio.
5. Adaptar o formulario para chamar a nova classe.
6. Confirmar build.
7. Repetir em pequenas etapas.

Evitar:

- mover grandes blocos de uma vez;
- misturar refatoracao estrutural com mudanca funcional ampla;
- duplicar logica em nome de "entregar rapido".