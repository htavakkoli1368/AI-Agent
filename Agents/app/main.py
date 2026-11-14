from contextlib import asynccontextmanager
from dotenv import load_dotenv
from fastapi import FastAPI, HTTPException
from agents import Runner
from app.agents.commerce_agent import commerce_agent
from app.schemas.chat import ChatRequest, ChatResponse


load_dotenv(override=True)


@asynccontextmanager
async def lifespan(app: FastAPI):
    yield


app = FastAPI(
    title="Commerce Assistant Agent Service",
    version="1.0.0",
    lifespan=lifespan
)


@app.get("/health")
async def health():
    return {"status": "healthy"}


@app.post("/api/chat", response_model=ChatResponse)
async def chat(request: ChatRequest):

    try:
        result = await Runner.run(
            commerce_agent,
            request.message
        )
    
        return ChatResponse(
            answer=result.final_output
        )

    except Exception as exc:
        import traceback
        traceback.print_exc()
        raise HTTPException(
            status_code=500,
            detail="Agent execution failed."
        ) from exc