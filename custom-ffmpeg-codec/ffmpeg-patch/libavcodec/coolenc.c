/*
 * COOL image format encoder. Designed to work with FFmpeg
 * Based on the BMP encoder.  
 * This encoder only supports the RGB8 pixel format
 *
 * Colin Dunn and Max Hanson 
 * CS 3505 FFmpeg Part 2
 * Feb 28, 2019
 */

#include "libavutil/imgutils.h"
#include "libavutil/avassert.h"
#include "avcodec.h"
#include "bytestream.h"
#include "internal.h"

/*
 * Initializes encoding into COOL file format given a CodecContext
 * Returns 0 if the pixel format is RGB8, returns an error otherwise.
 */
static av_cold int cool_encode_init(AVCodecContext *avctx){

  if(avctx->pix_fmt == AV_PIX_FMT_RGB8)
    avctx->bits_per_coded_sample = 8;
  else{
    av_log(avctx, AV_LOG_INFO, "unsupported pixel format\n");
    return AVERROR(EINVAL);
  }

  return 0;
}

/*
 * Encodes the data from an AVFrame into an AVPacket along with a header 
 * for the COOL file format.  Sets got_packet to 1 if successful.
 */
static int cool_encode_frame(AVCodecContext *avctx, AVPacket *pkt,
                            const AVFrame *p_frame, int *got_packet)
{
  /* Cool encoding only supports RGB8 at 8 bits per pixel
    Cool files include an 18 byte header of metadata */

  #define BITS_PER_PIXEL 8
  #define SIZE_HEADER 18

  int image_size, file_size, allocated, row, width, height;
  uint8_t *pict_input, *pict_output;

  width = avctx->width;
  height = avctx->height;

  /* Calculate file size so space can be allocated within the Packet */
  image_size = height * width;
  file_size = image_size + SIZE_HEADER;

  /* Allocate space in the AVPacket to write image data, return an error 
    if allocation fails */
  allocated = ff_alloc_packet2(avctx, pkt, file_size, 0);
  if (allocated < 0) {
     av_log(avctx, AV_LOG_ERROR, "FAILED TO ALLOCATE AVPACKET\n");
     return allocated;
  }
  /******************************************************************
   *                WRITING THE HEADER TO THE PACKET                *
   ******************************************************************/

  pict_output = pkt->data;
  bytestream_put_byte(&pict_output, 'C');                   
  bytestream_put_byte(&pict_output, 'O');                   
  bytestream_put_byte(&pict_output, 'O');
  bytestream_put_byte(&pict_output, 'L');

  bytestream_put_le32(&pict_output, SIZE_HEADER);                
  bytestream_put_le32(&pict_output, width);        
  bytestream_put_le32(&pict_output, height);         
  bytestream_put_le16(&pict_output, BITS_PER_PIXEL); 

  /******************************************************************
   *                    FINISHED THE INFO HEADER                    * 
   *              WRITING THE IMAGE DATA TO THE PACKET              *
   ******************************************************************/         

  /* point input to the start of the picture data */
  pict_input = p_frame->data[0];

  for(row = 0; row < height; row++) {
    memcpy(pict_output, pict_input, width);
    pict_output += width;
    pict_input += p_frame->linesize[0];
  }

  pkt->flags |= AV_PKT_FLAG_KEY;
  *got_packet = 1;
  return 0;
}

AVCodec ff_cool_encoder = {
    .name           = "cool",
    .long_name      = NULL_IF_CONFIG_SMALL("COOL image (CS 3505 Spring 2019)"),
    .type           = AVMEDIA_TYPE_VIDEO,
    .id             = AV_CODEC_ID_COOL,
    .init           = cool_encode_init,
    .encode2        = cool_encode_frame,
    .pix_fmts       = (const enum AVPixelFormat[]){
    AV_PIX_FMT_RGB8,
    AV_PIX_FMT_NONE
    },
};
