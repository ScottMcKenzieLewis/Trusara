# Trusara

.NET engineering tool for modeling and simulating pipe extrusion processes.

Trusara is a portfolio project exploring software design for manufacturing and industrial process control systems.  
The goal is to model key aspects of pipe extrusion lines such as temperature zones, line speeds, pressures, and machine state in a clean, testable domain model.

The project focuses on domain-driven design, simulation, and PLC-style process abstractions commonly found in industrial automation systems.

---

## Project Goals

Trusara is intended to explore several engineering software concepts:

- Process modeling for pipe extrusion lines
- Variable temperature zone control
- Process state evaluation and risk detection
- Simulation of manufacturing equipment behavior
- Clean domain architecture suitable for industrial software
- Test-driven development with property-based testing

---

## Features (Planned)

- Extrusion process domain model
- Variable temperature zone support
- Process state and telemetry modeling
- Risk assessment engine for detecting process deviations
- Simulation of extrusion line behavior
- PLC communication abstraction layer
- OPC UA / industrial protocol integration (future)

---

## Architecture

The solution is organized to keep industrial logic separate from infrastructure.

```
Trusara
├── Trusara.Domain # Core extrusion process domain model
├── Trusara.Plc.Abstractions # Interfaces for PLC communication
├── Trusara.Plc.Simulated # Simulated PLC for development and testing
└── Trusara.Domain.Tests # Unit and property tests
```


Key principles:

- **Domain-first design**
- **Infrastructure isolation**
- **Testability**
- **Clear engineering semantics**

---

## Domain Concepts

The domain model includes concepts commonly found in extrusion systems:

- Extrusion recipes
- Process setpoints
- Machine readings / telemetry
- Temperature zones
- Melt pressure
- Screw RPM
- Line speed
- Process risk flags

The domain layer evaluates machine readings against recipe targets to identify potential process issues.

---

## Testing

The project uses a modern .NET testing stack:

- **xUnit** for unit tests
- **FluentAssertions** for expressive assertions
- **FsCheck** for property-based testing

Property tests verify mathematical invariants such as:

- zone temperature deviation detection
- tolerance boundaries
- invariance across variable zone counts

---

## Status

Early development.

Current focus:

- Domain model
- Risk assessment engine
- Property-based testing

Future work will expand into simulation and PLC integration.

---

## Motivation

Industrial software often lives at the intersection of:

- engineering
- control systems
- manufacturing operations

Trusara explores how modern .NET architecture patterns can be applied to industrial engineering tools while keeping the domain model clear and testable.

---

## Related Projects

Other engineering software portfolio projects:

### Castara
Cast iron hardness and composition estimator.

https://github.com/ScottMcKenzieLewis/Castara

### Mensara
Python command-line tool for pipe calculations.

https://github.com/ScottMcKenzieLewis/Mensara