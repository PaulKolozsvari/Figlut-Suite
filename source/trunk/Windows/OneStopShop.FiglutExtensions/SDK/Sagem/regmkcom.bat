@echo off
set PATH=%Path%;%WINDIR%\Microsoft.NET\Framework\v2.0.50727
if not "%1"=="/u" (
	regasm /codebase Sagem.MorphoKit.dll
	regasm /codebase Sagem.ImageLibrary.dll
	regasm /codebase Sagem.MorphoKit.AcquisitionComponent.dll
	regasm /codebase ActiveMKit_Enroll.dll
	regasm /codebase Morpho.MorphoKit_FVP.dll
	regasm /codebase Morpho.MorphoAcquisition.dll
) else (
	regasm /unregister Morpho.MorphoAcquisition.dll
	regasm /unregister Morpho.MorphoKit_FVP.dll
	regasm /unregister ActiveMKit_Enroll.dll
	regasm /unregister Sagem.MorphoKit.AcquisitionComponent.dll
	regasm /unregister Sagem.ImageLibrary.dll
	regasm /unregister Sagem.MorphoKit.dll
)
pause
