# ⚔️ Mazmorra del Destino

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Console](https://img.shields.io/badge/Console-000000?style=for-the-badge&logo=windows-terminal&logoColor=white)

**Un Roguelite de Aventuras Épicas desarrollado en C# para consola**

*Explora mazmorras procedurales, derrota enemigos, colecciona loot épico y conviértete en una leyenda*

---

## 📖 Descripción

**Mazmorra del Destino** es un juego roguelite por turnos desarrollado completamente en C# para consola. El jugador debe atravesar 5 salas procedurales llenas de enemigos, recolectar objetos con sistema de rareza, enfrentarse a jefes épicos con habilidades especiales, y gestionar recursos estratégicamente en una tienda entre salas.

### 🎮 Género
- **Roguelite** - Muerte permanente con progresión entre partidas
- **RPG por turnos** - Combate táctico estratégico
- **Dungeon Crawler** - Exploración de mazmorras procedurales

---

## ✨ Características

### 🎯 Sistema de Combate
- ⚔️ **Combate por turnos estratégico** con múltiples acciones
- 🛡️ Mecánica de defensa temporal
- 🏃 Sistema de huida con probabilidad
- 💥 Ataques y contraataques dinámicos
- 🎲 Cálculo de daño con stats y bendiciones

### 👤 Sistema de Personajes
- 🦸 **Múltiples héroes jugables** con stats únicos
- ⭐ Sistema de experiencia y subida de nivel
- 💪 Mejora de stats al subir de nivel (+20 Vida, +2 Ataque)
- 🎒 Inventario con gestión de objetos
- ✨ Sistema de bendiciones activas

### 💎 Sistema de Loot
- 🎲 **4 niveles de rareza** con probabilidades balanceadas:
  - **Común** (60%) - Gris
  - **Raro** (30%) - Cyan
  - **Épico** (8%) - Magenta
  - **Legendario** (2%) - Amarillo
- 🌟 Objetos con efectos únicos
- 🎨 Visualización de rareza con colores y estrellas
- ⚖️ Sistema balanceado de valores

### 💰 Sistema Económico
- 💵 **Sistema de oro** ganado al derrotar enemigos
- 🏪 **Tienda itinerante** cada 3 salas
- 📊 Precios dinámicos basados en rareza
- 💸 Compra y venta de objetos
- 🤑 Bosses otorgan 100-200 oro

### 👹 Enemigos y Bosses
- 🎲 **Generación procedural** de enemigos por sala
- 👑 **Jefes finales épicos** con habilidades especiales:
  - 🔥 Ataques de área (ignoran defensa)
  - 💚 Regeneración de vida
  - ⚡ Habilidades que se activan cada X turnos
  - 🔄 Fase 2 con mecánicas mejoradas
- 📊 73+ enemigos únicos cargados desde JSON
- 💀 Sistema de tracking de enemigos derrotados

### 🗺️ Salas y Exploración
- 🏰 **5 salas procedurales** por partida
- 🚪 Transiciones cinemáticas entre salas
- 📦 Objetos dispersos en las salas
- ✨ Sistema de bendiciones post-combate
- 🎭 Descripciones ambientales únicas

### 🎨 Interfaz Visual Épica
- 🌈 **ASCII Art elaborado** con efectos visuales
- ⚡ **Animaciones fluidas** y transiciones cinematográficas
- 📊 **Barras de vida visuales** con gradientes de color
- 🎬 **Efectos de partículas** ASCII
- 💫 **Degradados de color** consistentes
- ✨ **Separadores decorativos** y marcos temáticos
- 🏆 **Sistema de ranking** con clasificación final

### 📊 Sistema de Estadísticas
- 📈 Tracking de daño total infligido
- 💀 Contador de enemigos derrotados por tipo
- 🎒 Inventario con objetos recolectados
- ⭐ Nivel alcanzado
- 💰 Oro acumulado
- 🏆 **Puntuación final** calculada
- 🎖️ **Rankings**: Leyenda Inmortal, Héroe Épico, etc.

---

## 🛠️ Tecnologías

- **Lenguaje:** C# 12
- **Framework:** .NET 8.0
- **Arquitectura:** Clean Architecture con servicios
- **Serialización:** System.Text.Json
- **Patrones:** Service Layer, Repository, MVC

---

## 📦 Instalación

### Requisitos Previos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- Visual Studio 2022 / Visual Studio Code / Rider

### Pasos de Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/mazmorra-del-destino.git
cd mazmorra-del-destino
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Compilar el proyecto**
```bash
dotnet build
```

4. **Ejecutar el juego**
```bash
dotnet run
```

---

## 🎮 Cómo Jugar

### Inicio del Juego
1. Ejecuta el programa
2. Selecciona tu héroe (Guerrero, Mago, etc.)
3. ¡Comienza tu aventura!

### Controles en Combate
- **[1]** - ⚔️ Atacar enemigo
- **[2]** - 🧪 Usar objeto del inventario
- **[3]** - 🛡️ Defender (reduce daño siguiente turno)
- **[4]** - 📦 Recoger objeto de la sala
- **[5]** - 🏃 Intentar huir (50% probabilidad)

### Fase de Exploración
- **[1]** - 📦 Recoger objetos restantes
- **[2]** - ✨ Aplicar bendición (una por sala)
- **[3]** - 🚪 Continuar a siguiente sala

### La Tienda
- Aparece cada 3 salas
- Compra objetos con oro ganado
- Vende objetos a mitad de precio
- Objetos con rareza mayor = más caros

### Consejos Estratégicos
💡 **Gestiona tu inventario:** No recojas todo, prioriza objetos de rareza alta
💡 **Guarda el oro:** Las pociones legendarias valen su precio
💡 **Usa bendiciones sabiamente:** Solo puedes usar una por sala
💡 **Defiende estratégicamente:** Especialmente contra bosses
💡 **Sube de nivel:** +20 Vida y +2 Ataque por nivel

---

## 🏗️ Arquitectura del Proyecto

### Estructura de Carpetas

```
RogueLite/
├── 📁 Data/                    # Archivos JSON con datos del juego
│   ├── Bosses/
│   │   └── bosses.json        # Definición de jefes finales
│   ├── Enemigos/
│   │   └── enemigos.json      # 73+ enemigos únicos
│   ├── Loot/
│   │   └── loot.json          # Objetos con rareza
│   ├── Bendiciones/
│   │   └── bendiciones.json   # 62 bendiciones
│   ├── Maldiciones/
│   │   └── maldiciones.json   # 66 maldiciones
│   └── Personajes/
│       └── personajes.json    # Héroes jugables
│
├── 📁 Models/                  # Modelos de datos
│   ├── Personaje.cs           # Héroe del jugador
│   ├── Enemigo.cs             # Enemigos base
│   ├── Boss.cs                # Jefes con habilidades
│   ├── Objeto.cs              # Items con rareza
│   ├── Sala.cs                # Salas de la mazmorra
│   ├── Bendicion.cs           # Buffs temporales
│   ├── Maldicion.cs           # Debuffs
│   └── Tienda.cs              # Sistema de comercio
│
├── 📁 Services/                # Lógica de negocio
│   ├── DataLoaderService.cs   # Carga de JSON
│   ├── CombatService.cs       # Sistema de combate
│   ├── LootService.cs         # Generación de loot
│   ├── PlayerService.cs       # Gestión del jugador
│   ├── RoomGeneratorService.cs # Generación procedural
│   ├── ItemEffectProcessor.cs  # Efectos de objetos
│   └── ResultadoTurno.cs      # Resultados de acciones
│
├── 📁 Manager/                 # Orquestadores
│   └── GameManager.cs         # Controlador principal
│
├── 📁 Controllers/             # Controladores de flujo
│   ├── InputHandler.cs        # Procesamiento de inputs
│   └── TiendaController.cs    # Lógica de la tienda
│
├── 📁 UI/                      # Interfaz de usuario
│   ├── Screens/               # Pantallas principales
│   │   ├── StartScreen.cs     # Pantalla de inicio épica
│   │   ├── CharacterSelectionScreen.cs # Selección de héroe
│   │   ├── GameScreen.cs      # Pantalla de juego
│   │   └── ResultScreen.cs    # Victoria/Derrota
│   ├── Renderers/             # Renderizadores específicos
│   │   ├── PlayerRenderer.cs  # Info del jugador
│   │   ├── RoomRenderer.cs    # Info de la sala
│   │   ├── CombatRenderer.cs  # Efectos de combate
│   │   └── MessageRenderer.cs # Mensajes del juego
│   ├── Components/            # Componentes reutilizables
│   │   ├── TextAnimator.cs    # Animaciones de texto
│   │   ├── HealthBarRenderer.cs # Barras de vida
│   │   └── DamageRenderer.cs  # Visualización de daño
│   └── UIManager.cs           # Orquestador de UI
│
└── Program.cs                 # Punto de entrada
```

### Patrones de Diseño Utilizados

#### 🏛️ **Service Layer Pattern**
- Separación de lógica de negocio en servicios especializados
- `CombatService`, `LootService`, `DataLoaderService`, etc.

#### 🎮 **MVC (Model-View-Controller)**
- **Models:** Clases de datos (`Personaje`, `Enemigo`, etc.)
- **Views:** UI/Screens y Renderers
- **Controllers:** `InputHandler`, `TiendaController`

#### 🎯 **Dependency Injection**
- Los servicios se inyectan en constructores
- Facilita testing y mantenimiento

#### 📊 **Repository Pattern**
- `DataLoaderService` abstrae el acceso a datos JSON
- Fácil cambio a base de datos en el futuro

#### 🎨 **Strategy Pattern**
- `ItemEffectProcessor` procesa efectos dinámicamente
- Fácil añadir nuevos tipos de objetos

---

## 📊 Datos del Juego

### Contenido Actual
- 👹 **73 enemigos únicos**
- 💎 **12 objetos con rareza**
- ✨ **62 bendiciones**
- 💀 **66 maldiciones**
- 👑 **2 jefes finales épicos**
- 🦸 **3+ héroes jugables**

### Configuración de Rareza
| Rareza | Probabilidad | Color | Estrellas |
|--------|-------------|-------|-----------|
| Común | 60% | Gris | ★ |
| Raro | 30% | Cyan | ★★ |
| Épico | 8% | Magenta | ★★★ |
| Legendario | 2% | Amarillo | ★★★★ |

---

## 🎯 Sistema de Puntuación

La puntuación final se calcula como:
```
Puntuación = Daño Total + (Enemigos × 50) + (Nivel × 100)
```

### Rankings
- 🏆 **Leyenda Inmortal** - 2000+ puntos
- ⭐ **Héroe Épico** - 1500+ puntos
- 💪 **Guerrero Valiente** - 1000+ puntos
- ⚔️ **Aventurero Prometedor** - 500+ puntos
- 🗡️ **Aprendiz Determinado** - < 500 puntos

---

## 🔮 Roadmap / Futuras Características

### En desarrollo
- [ ] Más clases de personajes con habilidades únicas
- [ ] Sistema de logros/achievements persistentes
- [ ] Reliquias pasivas permanentes
- [ ] Eventos aleatorios en salas
- [ ] Modo difícil con enemigos mejorados

### Planeado
- [ ] Sistema de seeds reproducibles
- [ ] Meta-progresión entre partidas
- [ ] Más tipos de salas (tesoro, eventos, descanso)
- [ ] Combos y sinergias entre objetos
- [ ] Sistema de crafteo
- [ ] Multiplayer local

---

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Si quieres mejorar el juego:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add: Amazing Feature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Áreas donde puedes contribuir
- 🎨 Nuevos efectos visuales ASCII
- 👹 Más enemigos y bosses
- 💎 Nuevos objetos legendarios
- 🎮 Mecánicas de combate adicionales
- 🐛 Corrección de bugs
- 📝 Documentación

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

---

## 👨‍💻 Autor

**Tu Nombre**
- GitHub: [dfernandezpozo](https://github.com/dfernandezpozo)
- Email: dfernandezpozo@iessonferrer.net

---

## 🙏 Agradecimientos

- Inspirado en clásicos roguelikes como *Slay the Spire*, *Darkest Dungeon* y *Hades*
- ASCII Art y efectos visuales inspirados en juegos retro de consola
- Comunidad de .NET y C# por las excelentes herramientas

---

**⚔️ ¡Que la fortuna te acompañe en la mazmorra! ⚔️**

*Hecho con ❤️ y mucho ☕*