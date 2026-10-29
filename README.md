# CommerceAssistant

**An AI-powered commerce assistant built with ASP.NET Core, Clean Architecture, SQL Server, and OpenAI Agents SDK.**

CommerceAssistant is a portfolio project that explores how to combine a traditional e-commerce backend with AI agents in a maintainable, secure, and extensible architecture.

The project separates the web interface, business API, application logic, data access, and AI agent runtime into distinct components. The goal is to build a foundation for an AI assistant that can interact with commerce data through controlled backend operations rather than accessing the database directly.

> **Project status:** Under active development. The architecture and core commerce features are being implemented incrementally. AI agent integration and additional capabilities will be added in later stages.

## Goals

- Apply Clean Architecture principles in an ASP.NET Core application.
- Build a maintainable commerce backend using C# and SQL Server.
- Separate presentation, business logic, and infrastructure concerns.
- Integrate Python-based AI agents using the official OpenAI Agents SDK.
- Expose controlled tools that allow AI agents to interact with backend capabilities.
- Explore agent orchestration, tool calling, guardrails, handoffs, tracing, and secure execution of business operations.
- Practice production-oriented concerns such as validation, authorization, data integrity, and concurrency.

## Architecture

The solution is designed around three main components:

```text
CommerceAssistant/
│
├── CommerceAssistant.Mvc/
│   └── ASP.NET Core MVC web application
│
├── CommerceAssistant.Api/
│   └── ASP.NET Core Web API
│
├── CommerceAssistant.Domain/
│   └── Entities and core business rules
│
├── CommerceAssistant.Application/
│   └── Use cases, DTOs, service contracts,
│       and repository interfaces
│
├── CommerceAssistant.Infrastructure/
│   └── EF Core, SQL Server, and data access
│
└── CommerceAssistant.Agents/
    └── Python service using OpenAI Agents SDK
        (planned)
```

The Python agent service will be developed as a separate service, potentially exposing its functionality through FastAPI.

### Component responsibilities

| Component | Responsibility |
|---|---|
| MVC | Web UI and user interaction |
| Web API | HTTP endpoints, authentication, authorization, and request handling |
| Domain | Commerce entities and business invariants |
| Application | Use cases, DTOs, and application contracts |
| Infrastructure | Database access and external technical integrations |
| Python Agent Service | Agent orchestration, tool calling, and AI-specific workflows |

### Clean Architecture

The .NET solution follows a pragmatic Clean Architecture approach.

- **Domain** contains core entities and business rules without depending on infrastructure frameworks.
- **Application** defines application use cases and the interfaces they require.
- **Infrastructure** implements technical concerns such as persistence and repository interfaces.
- **API** acts as the application entry point and composition root, configuring dependency injection and exposing HTTP endpoints.
- **MVC** communicates with the API over HTTP instead of accessing the database directly.

The intended dependency direction keeps the Domain independent of external technologies and prevents business logic from becoming tightly coupled to SQL Server or the web framework.

## AI Agent Integration

The planned AI layer will use Python and the OpenAI Agents SDK.

The agent service is intended to communicate with the .NET API through explicitly defined HTTP endpoints and tools.

The agent will not access SQL Server or the application's DbContext directly. Commerce operations will remain under the control of the backend, where business rules, authorization, and data integrity can be enforced.

Planned areas of exploration include:

- Tool calling and backend integration
- Agent instructions and structured outputs
- Input and output guardrails
- Handoffs between specialized agents
- Agent-as-tool patterns
- Tracing and debugging
- Multi-step workflows
- Authorization and safe execution of sensitive operations

These capabilities will be documented as they are implemented and tested.

## Planned Commerce Features

The initial domain focuses on the following entities:

- **Product:** Product information, SKU, price, and activation status.
- **Inventory:** Stock levels, reservations, and available quantities.
- **Order:** Customer orders and order lifecycle.
- **OrderItem:** Products, quantities, and historical unit prices associated with an order.

Potential application features include:

- Product creation, updates, search, and filtering
- Inventory availability and stock management
- Order creation and status tracking
- Customer-specific order access
- AI-assisted product discovery
- Natural-language commerce queries
- Controlled agent execution of supported backend operations

Features will be marked as available only after implementation and verification.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# / .NET | Core application development |
| ASP.NET Core MVC | Web interface |
| ASP.NET Core Web API | Backend API |
| Clean Architecture | Separation of concerns |
| Entity Framework Core | ORM and persistence |
| SQL Server | Relational database |
| Python | AI agent service |
| OpenAI Agents SDK | Agent orchestration and tool calling |
| FastAPI | Planned Python service API |
| Git / GitHub | Version control and project collaboration |

## Security and Reliability Principles

The project is being designed with the following principles in mind:

- Validate requests at application boundaries.
- Enforce business invariants in the Domain where appropriate.
- Keep authentication and authorization under backend control.
- Verify customer ownership before returning or
