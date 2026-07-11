# Projeto Unity Mini-Game — Documentação de Decisões Técnicas e Arquitetura

### 1. Padrão de Projeto Singleton (Gerenciamento Central)
O ciclo de vida global do jogo é gerenciado por meio do padrão **Singleton** na classe `GameController` através da referência pública e estática `gc`.
* **Objetivo:** Centralizar o controle dos estados de cena (Introdução, Login, Regras, Gameplay, Contagem Regressiva e Ranking) e mediar a comunicação entre subsistemas secundários (`LightControl`, `DataManager`, `AudioManager`, `RankingPanel`, `ParticleManager`).
* **Inicialização:** A instância global é atribuída durante a chamada do método `Start()` (`gc = this;`), assegurando acesso unificado aos dados da sessão a partir de scripts externos.

### 2. Persistência de Dados Relacional (SQLite)
A persistência de dados utiliza a biblioteca `SQLite` operando como um mapeamento objeto-relacional (ORM) local.
* **Estrutura de Tabelas:** O banco de dados armazena as entidades `PlayerData` (com os campos `Id`, `Player`, `Email` e `CompanyName`) e `ScoreBoard` (com as colunas `Id`, `PlayerId`, `Time` e `Score`).
* **Tratamento de Conflitos:** O método `InsertPlayerData` utiliza expressões LINQ (`FirstOrDefault`) para verificar a existência prévia do registro do jogador[cite: 9]. Caso exista, executa um comando `Update` para atualizar os dados cadastrais; caso contrário, realiza um `Insert`.
* **Otimização de Consultas (Ranking):** O método `GetTopPlayers` executa uma subquery SQL estruturada para selecionar apenas o maior score de cada jogador individual (`ORDER BY score DESC, time ASC LIMIT 1`), aplicando posteriormente uma ordenação decrescente limitada pela variável `rankingSize`. Isso impede que um mesmo usuário ocupe múltiplas posições no painel de líderes.

### 3. Adaptação Dinâmica de Resolução (UI)
O posicionamento correto dos elementos de interface em dispositivos com diferentes proporções de tela é gerenciado pela classe `PortraitAnchor`.
* **Cálculo de Proporção:** O script compara em tempo real a proporção atual da tela (`Screen.width / Screen.height`) com o aspecto nativo configurado para o projeto (Modo Retrato — `9:16`).
* **Manipulação do RectTransform:** Com base no desvio detectado, os limites matemáticos de `anchorMin` e `anchorMax` são recalculados dinamicamente para aplicar técnicas de *Letterboxing* ou *Pillarboxing*, definindo os offsets como zero para travar a área útil de interação[cite: 4].

### 4. Instanciação e Layout Computados via Script
A tabela de classificação (`RankingPanel`) é construída de maneira dinâmica para evitar a dependência de objetos estáticos na hierarquia da cena.
* **Ajuste de Container:** O tamanho vertical do componente `Content` é calculado via código multiplicando a altura do prefab (`RankLabelPrefab`) pela quantidade definida de registros (`maxPlayersToShow`), adicionando os espaçamentos lineares (`spacing`).
* **Ancoragem:** Cada elemento instanciado recebe suas definições de pivô, âncora horizontal e posicionamento no eixo Y calculados sequencialmente através do laço de repetição, injetando os dados textuais tratados diretamente nos componentes filhos `TMP_Text`.

### 5. Subsistemas de Feedback e Iluminação 2D
* **Controle de Iluminação (URP Light2D):** A classe `LightControl` interage com o pipeline de renderização 2D da Unity. O raio externo das fontes de luz (`pointLightOuterRadius`) e suas posições nos eixos são ajustados proporcionalmente com base na propriedade `orthographicSize` da câmera principal. Os estados das luzes são alternados dinamicamente entre os layouts `Intro`, `Default` e `Countdown`.
* **Reutilização de Partículas:** O componente `ParticleManager` altera a propriedade `transform.position` de um único emissor de partículas para as coordenadas do botão acionado antes de invocar o método `Play()`, reduzindo o overhead associado à instanciação repetida de objetos.
* **Áudio Não-Bloqueante:** O `AudioManager` utiliza o método `PlayOneShot` para a reprodução de efeitos sonoros ocasionais, permitindo a execução simultânea de faixas de áudio e a manutenção da música de fundo em loop contínuo sem interrupções nas trilhas.

### 6. Lógicas de Jogo, Combo e Temporizadores
* **Cálculo do Multiplicador:** A pontuação obtida por acerto é indexada ao `scoreMultiplier`, determinado matematicamente em função da sequência atual (`comboCount`) dividida pela taxa pré-configurada (`hitPerCombo`), utilizando arredondamento para cima (`Mathf.CeilToInt`).
* **Monitoramento de Inatividade (AFK):** Através do ciclo `FixedUpdate`, o `GameController` decrementa continuamente uma variável de tempo (`afkTimer`) baseada em `Time.fixedDeltaTime`. Caso nenhuma interação resete o temporizador e ele atinja valor igual ou menor a zero, o estado do jogo é revertido automaticamente para a tela inicial (`ResetToIntroScene`).
* **Interação via Rich Text:** A classe `TitleMiniGame` valida cliques em caracteres individuais de um componente de texto através de `TMP_TextUtilities.FindIntersectingCharacter`. A atualização das cores dos caracteres utiliza a classe `StringBuilder` para concatenar as tags HTML de formatação (`<color>`), reduzindo a alocação de memória temporária e mitigando pressões sobre o coletor de lixo (Garbage Collector).
* **Gamificação e Engajamento na Interface Inicial (IntroMinigame):** Como decisão de design voltada à interatividade desde o primeiro contato do usuário, a tela de introdução implementa um minigame integrado ao próprio logotipo do jogo (`TitleMiniGame`). 
  * **Mecânica de Interação:** Utilizando a classe `TMP_TextUtilities.FindIntersectingCharacter`, o sistema intercepta os dados de clique do usuário (`PointerEventData`) e calcula com precisão matemática se a coordenada do ponteiro coincide com o caractere destacado pelo sistema. 
  * **Otimização de Memória:** Quando o caractere correto é acionado, o script sorteia um novo índice por meio de um laço `do-while` (evitando a repetição consecutiva do mesmo caractere). A reconstrução visual da string e a aplicação dinâmica das tags HTML de formatação (`<color=green>` / `<color=white>`) são processadas via `StringBuilder`. Essa abordagem evita a concatenação repetida de strings comuns, minimizando a alocação de memória temporária e reduzindo os ciclos de processamento do coletor de lixo (*Garbage Collector*).

---

## Fluxo de Execução Simplificado

1. **Inicialização:** O `DataManager` estabelece conexão com o arquivo SQLite e gera as estruturas das tabelas caso não existam. O `GameController` define o estado para a interface introdutória.
2. **Autenticação:** O `LoginManager` valida se o campo de texto obrigatório foi preenchido para habilitar o botão de início. Os dados são inseridos ou atualizados no banco local.
3. **Contagem Regressiva:** A corrotina `CountdownCoroutine` atualiza o indicador numérico a cada segundo regulado por `WaitForSeconds(1f)`, emitindo bipes sonoros e modificando o layout de luzes.
4. **Loop de Jogabilidade:** O sistema altera a cor de um elemento filho aleatório do painel de botões para verde através de uma verificação em laço `do-while` (que impede a repetição do mesmo índice consecutivamente). Cliques corretos disparam sistemas visuais, atualizam pontuações e reiniciam o temporizador AFK.
5. **Conclusão de Partida:** Ao zerar o tempo da partida, a média de tempo de reação é calculada, o score é enviado para o banco de dados e a interface do ranking é reconstruída com os dados atualizados extraídos do banco de dados.
## Tecnologias utilizadas

- **Unity:** 6000.5.2f1
- **Banco de dados:** SQLite

## Banco de dados

O projeto utiliza SQLite para armazenamento dos dados.

O arquivo do banco de dados é criado no seguinte caminho (Windows):
- %USERPROFILE%\AppData\LocalLow\Roberto Bevilaqua\Bataki-Branch77

## Integração SQLite no Unity

Para implementar o banco de dados SQLite dentro do Unity foi necessário utilizar a biblioteca:

SQLite-net para Unity  
https://github.com/gilzoide/unity-sqlite-net

Essa biblioteca permite realizar operações de criação, leitura, atualização e exclusão de dados utilizando SQLite diretamente no projeto Unity.

## Downloads

Versões compiladas do projeto estão disponíveis para Windows e Android:

- Windows Executable (.exe)
- Android APK

Download:

https://github.com/RBevilaquaC/Bataki-Branch77/releases/tag/v1
