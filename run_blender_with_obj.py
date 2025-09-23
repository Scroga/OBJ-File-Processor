import subprocess
import sys
import os

if len(sys.argv) != 3:
    print("Usage: python run_blender_with_obj.py path/to/blender.exe path/to/model.obj")
    sys.exit(1)

BLENDER_PATH = sys.argv[1]
OBJ_FILE = sys.argv[2]

if not os.path.isfile(BLENDER_PATH):
    raise FileNotFoundError(f"Blender.exe file not found: {BLENDER_PATH}")
if not os.path.isfile(OBJ_FILE):
    raise FileNotFoundError(f"OBJ file not found: {OBJ_FILE}")

IMPORT_SCRIPT = "import_obj.py"

script = f"""
import bpy
import addon_utils

addon_utils.enable("io_scene_obj", default_set=True, persistent=True)

bpy.ops.object.select_all(action='SELECT')
bpy.ops.object.delete(use_global=False)

obj_path = r"{OBJ_FILE}"

if bpy.app.version >= (4, 0, 0):
    bpy.ops.wm.obj_import(filepath=obj_path)
else:
    bpy.ops.import_scene.obj(filepath=obj_path)
"""

with open(IMPORT_SCRIPT, "w") as f:
    f.write(script)

subprocess.run([BLENDER_PATH, "--python", IMPORT_SCRIPT])