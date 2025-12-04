# Proyecto 3D en Unity - INACAP

Proyecto realizado en Unity 2022.3.62f2

## Abrir Proyecto
- Para abrir el proyecto dar click en importar proyecto desde UnityHub y seleccionar ésta carpeta, el programa deberia encargarse de seleccionar la version de Unity correcta para el proyecto y la importación de los Assets y Packages. Luego al entrar al proyecto en Unity 2022.3.62f2, abrir la escena MainScene en Assets/Scenes/

### Controles
- **A:** Girar camara a la izquierda.
- **D:** Girar camara a la derecha.
- **W:** Avanzar.
- **S:** Retroceder.
- **Space:** Rodar. (Solo al estar caminando o corriendo)
- **Shift:** Correr. (Solo al caminar hacia adelante)
- **Click izquierdo:** Ataque principal.
- **Click derecho:** Ataque secundario.
- **Esc:** Recargar la escena (Reaparecen los enemigos y colecionables).

### Mecánicas
- **Spawn aleatorio:** Al iniciar o reaparecer, se genera enemigos y coleccionables en ubicaciones aleatorias.

- **Ataque:** Con click izquierdo y derecho se ejecutan ataques que restan 1 de vida a los enemigos con cada impacto.

- **Planta curativa:** Las plantas otorgan 50 puntos y curan en 2 puntos de vida, hasta un maximo de 10.

- **Reaparición:** Cuando el jugador es eliminado, aparece en pantalla un boton de reiniciar, que recarga toda la escene, incluyendo enemigos y coleccionables.

- **Recargar escena:** Al presionar la tecla Escape, toda la escena se recarga, incluyendo los enemigos y colecionables.

### Enemigos
- **Rogue:** Tiene 3 puntos de vida. Se mueve de forma aleatoria, pero al detectar al jugador se acerca y ejecuta ataques que le quitan 1 punto de vida.

