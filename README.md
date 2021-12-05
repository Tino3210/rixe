# Rixe

## Description
Le jeu est une application WPF avec C#, qui se joue à deux joueurs. Ils doivent s'envoyer ou esquiver des projectiles sans la possibilité de voir son opposant sur l'écran. Pour envoyer un projectile sur l'écran de l'autre joueur, il faut qu'il traverse le haut de l'écran. À chaque coup reçu, le joueur perd un point de vie. La partie se termine quand un des deux joueurs à perdu ces trois vies.

## Spécification
- Le se déroule en réseau
- Contrôle du joueur
  - Déplacement (WASD)
  - Tir (ESPACE)
- Il y a un certain temps entre chaque tir
- Les joueurs ont trois vies
- Un projectile est envoyé chez l'autre joueur quand il franchit le haut bord de l'écran

## Context
Embrouille en pleine cité entre deux personnes. Les projectiles du jeu sont des objets se trouvant sur la voie publique (chaises, pierre, bouteilles).

## État de l'art
Le jeu s'inspire du jeu mobile [DUAL!](https://play.google.com/store/apps/details?id=com.Seabaa.Dual&hl=fr_CH&gl=US).
