/*
    Filename: compressImage.js
    Part of Project: PLOT/PLOT-FE/Plot/wwwroot/js

    File Purpose:
    This file contains an image compression implementation for
    the image uploading functionality. This will take the user's
    image as a byte array, print it onto a js canvas with a 
    specified quality, then crop the image down to a specified
    target width/height. Found this to be the easiest way to
    have the image inputs be around the same size and manageable.

    Written by: Jordan Houlihan
*/

window.compressImage = async function (
  byteArray,
  targetWidth,
  targetHeight,
  quality
) {
  const blob = new Blob([new Uint8Array(byteArray)], { type: "image/jpeg" });
  const img = new Image();

  return new Promise((resolve) => {
    const url = URL.createObjectURL(blob);
    img.onload = () => {
      const canvas = document.createElement("canvas");
      canvas.width = targetWidth;
      canvas.height = targetHeight;
      const ctx = canvas.getContext("2d");

      const imageRatio = img.width / img.height;
      const targetRatio = targetWidth / targetHeight;

      let sourceWidth, sourceHeight, sourceX, sourceY;

      if (imageRatio > targetRatio) {
        sourceHeight = img.height;
        sourceWidth = img.height * targetRatio;
        sourceX = (img.width - sourceWidth) / 2;
        sourceY = 0;
      } else {
        sourceWidth = img.width;
        sourceHeight = img.width / targetRatio;
        sourceX = 0;
        sourceY = (img.height - sourceHeight) / 2;
      }

      ctx.drawImage(
        img,
        sourceX,
        sourceY,
        sourceWidth,
        sourceHeight,
        0,
        0,
        targetWidth,
        targetHeight
      );

      canvas.toBlob(
        (compressedBlob) => {
          const reader = new FileReader();
          reader.onloadend = () => {
            resolve(new Uint8Array(reader.result));
          };
          reader.readAsArrayBuffer(compressedBlob);
        },
        "image/jpeg",
        quality
      );
    };
    img.src = url;
  });
};
