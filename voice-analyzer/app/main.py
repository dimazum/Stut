from fastapi import FastAPI, UploadFile, File
from analysis import analyze_voice_bytes
import datetime

app = FastAPI()

@app.post("/analyze")
async def analyze(file: UploadFile = File(...)):
    data = await file.read()

    result = analyze_voice_bytes(data)

    result["timestamp"] = datetime.datetime.utcnow().isoformat()
    result["file_name"] = file.filename

    return result
