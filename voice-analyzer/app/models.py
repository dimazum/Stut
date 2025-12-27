from pydantic import BaseModel
from uuid import UUID

class AudioChunkReceived(BaseModel):
    sessionId: UUID
    userId:UUID
    chunkIndex: int
    fileName: str
    contentType: str
    data: str  # base64
