from fastapi import FastAPI, Request
import subprocess

app = FastAPI()

@app.post("/execute_script")
async def execute_script(request: Request):
    data = await request.json()
    script = data.get("script")

    if not script:
        return {"status": "error", "message": "No script provided"}

    try:
        with open("temp_script.lua", "w") as file:
            file.write(script)
        subprocess.run(["injector.exe", "temp_script.lua"])

        return {"status": "success", "message": "Script executed successfully"}
    except Exception as e:
        return {"status": "error", "message": str(e)}
