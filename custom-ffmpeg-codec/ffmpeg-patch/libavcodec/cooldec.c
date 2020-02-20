/*
 * COOL image format decoder. Designed to work with FFmpeg
 * Based on the BMP decoder.  
 * This decoder only supports the RGB8 pixel format
 *
 * Colin Dunn and Max Hanson 
 * CS 3505 FFmpeg Part 2
 * Feb 28, 2019
 */

#include <inttypes.h>
#include "avcodec.h"
#include "bytestream.h"
#include "internal.h"
#include "msrledec.h"

/*
 * Decodes the data from an AVPacket into an AVFrame in the 
 * COOL file format.  Sets got_frame to 1 if successful.  
 */
static int cool_decode_frame(AVCodecContext *avctx, void *data,
    int *got_frame, AVPacket *avpkt)
{
    #define BITS_PER_PIXEL 8

    const uint8_t *pict_input, *pict_output;
    AVFrame *p_frame;
    int width, height, depth, header_size;
    int row, picture_linesize, buffer_flag;

    p_frame = data;
    pict_input = avpkt->data;

    /******************************************************************
     *                 READ THE HEADER OF THE FILE                    *
     *****************************************************************/
    /* check the magic numbers of the file */
    if (bytestream_get_byte(&pict_input) != 'C' ||
        bytestream_get_byte(&pict_input) != 'O' ||
        bytestream_get_byte(&pict_input) != 'O' ||
        bytestream_get_byte(&pict_input) != 'L') {
        av_log(avctx, AV_LOG_ERROR, "Bad magic numbers.\n");
        return AVERROR_INVALIDDATA;
    }

    /* retrieve other information from the header */
    header_size = bytestream_get_le32(&pict_input);
    width = bytestream_get_le32(&pict_input);
    height = bytestream_get_le32(&pict_input);
    depth = bytestream_get_le16(&pict_input);

    /* Cool file format only supports 8 bits per pixel */
    if(depth == BITS_PER_PIXEL){
        avctx->pix_fmt = AV_PIX_FMT_RGB8;
    }
    else{
        av_log(avctx, AV_LOG_ERROR, "INCORRECT PIXEL DEPTH\n");
        return AVERROR_INVALIDDATA;
    }

    /* set the context with information about the file */
    avctx->width = width;
    avctx->height = height;


    /*****************************************************************
     *              GET DATA FRAME FROM CODEC CONTEXT                *
     *****************************************************************/
    
    buffer_flag = ff_get_buffer(avctx, p_frame, 0);
    if (buffer_flag < 0){
        av_log(avctx, AV_LOG_ERROR, "FAILED TO GET BUFFER FROM CODEC CONTEXT\n");
        return buffer_flag;
    }

    /* Borrowed from BMP decoder */
    p_frame->pict_type = AV_PICTURE_TYPE_I;
    p_frame->key_frame = 1;


    /******************************************************************
     *                DECODE THE PIXELS OF THE IMAGE                  *
     *****************************************************************/

    /* copy from the buffer into the picture row by row */
    pict_output = p_frame->data[0];
    picture_linesize = p_frame->linesize[0]; 
    for (row = 0; row < height; row++) {
        memcpy(pict_output, pict_input, width);
        pict_input += width;
        pict_output += picture_linesize;
    }
    
    *got_frame = 1;
    return avpkt->size;
}

AVCodec ff_cool_decoder = {
    .name           = "cool",
    .long_name      = NULL_IF_CONFIG_SMALL("COOL image (CS 3505 Spring 2019)"),
    .type           = AVMEDIA_TYPE_VIDEO,
    .id             = AV_CODEC_ID_COOL,
    .decode         = cool_decode_frame,
    .capabilities   = AV_CODEC_CAP_DR1,
};
