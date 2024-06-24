# DLM Cube Solver

## Descrição
Este projeto consiste em um robô para a solução de cubos mágicos do tipo 3x3. O sistema inclui a estrutura física, circuitos eletrônicos e um aplicativo para interface homem-máquina.

## Objetivo
Criar um robô capaz de resolver cubos mágicos 3x3, projetando a estrutura, circuitos e desenvolvendo um aplicativo para interação com o usuário.

## Introdução
O cubo mágico é um quebra-cabeça tridimensional inventado por Ernő Rubik. Cada face pode ser rotacionada no sentido horário ou anti-horário. Este projeto utiliza a notação Singmaster para representar os movimentos do cubo.

## Materiais Utilizados
- **Motores e Drivers:**
  - 6 x Motor stepper 17HD2607-01H
  - 6 x Driver de motor stepper A4899
- **Componentes Eletrônicos:**
  - 1 x Fonte de alimentação de 12 V
  - 1 x Botão de interrupção
  - 1 x Capacitor para debounce RC de 15 nF
  - 1 x Regulador de tensão 12 V para 5 V KA7805
  - 1 x Microcontrolador ESP32-WROOM
  - 2 x Placa de fenolite 18 x 24 furos (5x7 cm)
  - 1 x Conector P4 fêmea
  - 6 x Capacitores de 100 μF
  - Cabos
  - Estação e materiais para solda
  - Cubos para teste
- **Ferramentas e Materiais para Estrutura:**
  - Impressora 3D e filamento PLA
  - Parafusos e chaves respectivas

## Desenvolvimento

### Estrutura
A estrutura foi projetada inicialmente no SketchUp e posteriormente no Fusion 360, utilizando perfis de alumínio estrutural e peças impressas em PLA.

### Circuito
O circuito foi projetado no EasyEDA e montado em placas de fenolite perfuradas. A placa contém os drivers de motor, regulador de tensão, e o microcontrolador ESP32-WROOM.

### Aplicativo
O aplicativo foi desenvolvido em C# utilizando .NET MAUI, permitindo que o usuário entre com o estado do cubo e envie a sequência de movimentos para o microcontrolador via Bluetooth.

## Algoritmo de Solução
Utilizamos o Two-Phase Algorithm, desenvolvido por Herbert Kociemba, que consiste em duas fases: a primeira fase orienta os cubies corretamente e a segunda fase resolve o cubo a partir desse estado. Mais detalhes podem ser encontrados no [repositório do algoritmo](https://github.com/Megalomatt/Kociemba).

## Resultados
O robô alcançou seus objetivos principais, sendo capaz de resolver cubos mágicos em uma média de 4 segundos por solução. Problemas de alinhamento foram solucionados através de refinamentos nas peças impressas em PLA.

## Conclusão
O projeto, apesar de não perfeito, é funcional e pode ser melhorado para comercialização. A experiência contribuiu para o desenvolvimento técnico e de trabalho em equipe dos participantes.

## Referências
- JPerm, "3x3 Rubik's Cube Moves", 2024. [Link](https://jperm.net/3x3/moves)
- Herbert Kociemba, "Cube Explorer", 2024. [Link](https://kociemba.org/cube.htm)
- Matt Colbourne, "Kociemba", 2024. [Link](https://github.com/Megalomatt/Kociemba)
- Autodesk, "Fusion 360", 2024. [Link](https://www.autodesk.com/products/fusion-360)
- Trimble Inc., "SketchUp", 2024. [Link](https://www.sketchup.com/pt-br)

## Autores
- Douglas Sonntag
- Lucas Dalle
- Marcos Rocha
