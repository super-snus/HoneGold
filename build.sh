#!/usr/bin/env bash
set -e

OUT="GameBuild"
DATA_DIR="$OUT/mem_GameData"

echo "==> 1. Очистка старой сборки..."
rm -rf "$OUT"
mkdir -p "$DATA_DIR/Assets"
mkdir -p "$DATA_DIR/Modules"

echo "==> 2. Сборка модулей..."
# Собираем DLL модулей отдельно в папку Modules
dotnet build HoneModule_RayLib-Render/HoneModule_RayLib-Render.csproj -c Release -o "$DATA_DIR/Modules" -p:DebugType=None
dotnet build HoneModule_Velcro-Physics/HoneModule_Velcro-Physics.csproj -c Release -o "$DATA_DIR/Modules" -p:DebugType=None

# Вычищаем зависимости, которые билд модулей мог скопировать за собой (HoneEngine, json-конфиги, pdb)
rm -f "$DATA_DIR/Modules"/HoneEngine.dll
rm -f "$DATA_DIR/Modules"/*.deps.json
rm -f "$DATA_DIR/Modules"/*.pdb

echo "==> 3. Сборка монолитного mem (всё зашито внутрь)..."
dotnet publish HoneRunner/HoneRunner.csproj \
  -c Release \
  -r linux-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:EnableCompressionInSingleFile=true \
  -p:DebugType=None \
  -p:DebugSymbols=false \
  -o "$OUT"

echo "==> 4. Копирование GameConfig.json..."
if [ -f "GameConfig.json" ]; then
  cp "GameConfig.json" "$DATA_DIR/"
elif [ -f "HoneRunner/GameConfig.json" ]; then
  cp "HoneRunner/GameConfig.json" "$DATA_DIR/"
fi

echo "==> 5. Копирование ассетов (исключая файлы *.cs)..."
if [ -d "HoneGame/Assets" ]; then
  if command -v rsync >/dev/null 2>&1; then
    rsync -av --exclude="*.cs" HoneGame/Assets/ "$DATA_DIR/Assets/"
  else
    (cd HoneGame/Assets && find . -type f ! -name "*.cs" | cpio -pdm "../../../$DATA_DIR/Assets")
  fi
fi

echo "==> 6. Вычищаем отладочные файлы из корня..."
rm -f "$OUT"/*.pdb "$OUT"/*.dbg "$OUT"/*.deps.json

echo "==> Готово! Структура папок:"
tree "$OUT" 2>/dev/null || find "$OUT" -print | sed -e 's;[^/]*/;|____;g;s;____|; |;g'