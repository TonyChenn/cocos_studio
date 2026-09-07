# Mono.TextEditor source provenance

This directory restores the source corresponding to the repository's legacy
`Mono.TextEditor.dll`. The source files were copied without reformatting from
MonoDevelop commit `48d16bc4f12ce3938964fc7c3d72fdc6887ad4ad`
(`monodevelop-5.9.5.10`), directory `main/src/core/Mono.Texteditor`.

The same Mono.TextEditor tree (`ea03c8f2513e69f82d79cb38e9c826bfb87f4401`)
is present in MonoDevelop 5.9.5.10, 5.9.6.65, 5.9.7.22 and 5.9.8.0. The
legacy DLL has PE timestamp `2015-09-15 02:27:53Z`; it contains the 5.9-only
editor changes and does not contain the later 5.10 changes. This makes the 5.9
stable tree a closer source baseline than the previously documented 5.4 tag.

All 30 syntax-mode XML files, eight style JSON files and `gtk-gui/gui.stetic`
are byte-identical to the 39 resources embedded in the legacy DLL. The imported
C# files retain their original authorship, copyright, licence and explanatory
comments. `LICENSE.txt` collects the licence text while the notices in each
source file remain authoritative. No decompiled C# file was copied into this
directory.

The upstream directory contains 161 C# files, but its 5.9 project compiles 159.
`Mono.TextEditor/ITextPasteHandler.cs` and
`Mono.TextEditor/Gui/SolidFoldMarkerMargin.cs` are retained as upstream history
and explicitly excluded from compilation, matching the official project.

`obj/MonoTextEditorAudit/original` remains an ignored comparison input only. It
was used to confirm the assembly surface, version boundary and resource bytes.
The per-file classification is recorded in `SOURCE_MANIFEST.tsv`:

- `upstream-source`: exact upstream source, including original comments;
- `upstream-resource/current-dll-byte-identical`: exact upstream resource whose
  SHA-256 also matches the resource extracted from the legacy DLL;
- `upstream-config/runtime-asset-byte-identical`: upstream dllmap configuration,
  already shipped from `runtime-assets` with the same SHA-256.

No Cocos-specific Mono.TextEditor source change has been identified. The known
Cocos editor hooks (`ProcessSaveText`, `ProcessLoadText` and
`PrepareToSetCaret`) belong to the separate SourceEditor2 consumer and are not
overwritten here.

The restored source has passed Debug/Release x86 cold builds, assembly/API/
reference/native-entry comparison, automated behavior regression and user
interaction acceptance. The legacy `dlls/Mono.TextEditor.dll` was then removed;
the fixed Git baseline remains available only to hash-checked comparison tests.
