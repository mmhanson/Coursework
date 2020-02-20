/**
 * This program uses FFMPEG to draw an animation of a bouncing ball over a
 * jpeg picture. Frames are encoded and written in a custom codec, the 'cool'
 * codec added into FFMPEG itself.
 *
 * This program was written by Maxwell Hanson and Colin Dunn, March 2019
 *
 * This program includes code snippets and inspiration from the following:
 *  - Dranger's FFMPEG tutorial (dranger.com/ffmpeg/tutorial01.html)
 *  - FFMPEG's decoding_encoding.c example (https://ffmpeg.org/doxygen/trunk/
 *    decoding_encoding_8c-source.html)
 *  - FFMPEG's transcoding.c example (https://ffmpeg.org/doxygen/trunk/
 *    doc_2examples_2transcoding_8c-example.html)
 */

extern "C" {
#include "../ffmpeg/libavcodec/avcodec.h"
#include "../ffmpeg/libavformat/avformat.h"
#include "../ffmpeg/libswscale/swscale.h"
}

#include <iostream>
#include <string>

using std::cout;
using std::cin;
using std::cerr;
using std::endl;
using std::string;


/*
 * Takes an AVFormatContext and finds the appropriate decoder
 * Uses the decoder to open an AVCodecContext
 * Returns the AVCodecContext
 * This code is based on an online tutorial - dranger.com/ffmpeg/tutorial01.html
 */
static AVCodecContext* getCodecContext(AVFormatContext* pFormatCtx)
{
  AVCodecContext    *pCodecCtxOrig = NULL;
  AVCodec           *pCodec = NULL;
  AVCodecContext    *pCodecCtx = NULL;

  // Get a pointer to the codec context for the video stream
  pCodecCtxOrig=pFormatCtx->streams[0]->codec;

  // Get the decoder associated with the image
  pCodec=avcodec_find_decoder(pCodecCtxOrig->codec_id);

  // Copy the original context and open the codec
  pCodecCtx = avcodec_alloc_context3(pCodec);
  avcodec_copy_context(pCodecCtx, pCodecCtxOrig);
  avcodec_open2(pCodecCtx, pCodec, NULL);

  return pCodecCtx;
}

/*
 * Takes an AVFrame and a specified pixel format and scales
 * the original frame into the format specified
 * Returns the scaled frame
 * This code is based on an online tutorial - dranger.com/ffmpeg/tutorial01.html
 */
static AVFrame* scaleFrame(AVFrame* originalFrame, AVPixelFormat pixFmt)
{
  struct SwsContext *sws_ctx = NULL;
  int numBytes;
  uint8_t* buffer = NULL;
  AVFrame* scaledFrame = NULL;

  // allocate a new frame to hold the scaled data
  scaledFrame = av_frame_alloc();

  // Determine required buffer size and allocate buffer
  numBytes=avpicture_get_size(AV_PIX_FMT_RGB24, originalFrame->width, originalFrame->height);
  buffer=(uint8_t *)av_malloc(numBytes*sizeof(uint8_t));
  
  // Assign appropriate parts of buffer to image planes in scaledFrame
  avpicture_fill((AVPicture *)scaledFrame, buffer, AV_PIX_FMT_RGB24,
		 originalFrame->width, originalFrame->height);

  // initialize SWS context for software scaling
  sws_ctx = sws_getContext(originalFrame->width, originalFrame->height, pixFmt,
			   originalFrame->width, originalFrame->height, AV_PIX_FMT_RGB24,
			   SWS_BILINEAR, NULL, NULL, NULL );

  // Use SWS context to scale originalFrame into scaledFrame
  sws_scale(sws_ctx, (uint8_t const * const *)originalFrame->data,
            originalFrame->linesize, 0, originalFrame->height,
	    scaledFrame->data, scaledFrame->linesize);

  // sws_scale doesn't set parameters of scaled frame, so update them here
  scaledFrame->height = originalFrame->height;
  scaledFrame->width = originalFrame->width;
  scaledFrame->format = originalFrame->format;

  return scaledFrame;
}

/*
 * Open the specified file and decode the image
 * Scale the decoded image into RGB24 format and return the scaled frame
 * This code is based on an online tutorial - dranger.com/ffmpeg/tutorial01.html
 */
static AVFrame* makeDecodedFrame(char* filename)
{
  AVFormatContext   *pFormatCtx = NULL;
  int               frameFinished;  
  AVCodecContext    *pCodecCtx = NULL;  
  AVFrame           *pFrame = NULL;
  AVFrame           *pFrameRGB = NULL;
  AVPacket          packet;

  // Open file and find the stream info
  avformat_open_input(&pFormatCtx, filename, NULL, NULL);
  avformat_find_stream_info(pFormatCtx, NULL);

  // Get and open a codec context from the AVFormatContext
  pCodecCtx = getCodecContext(pFormatCtx);
 
  // Allocate frame for decoding
  pFrame = av_frame_alloc();

  while(av_read_frame(pFormatCtx, &packet)>=0) {
      avcodec_decode_video2(pCodecCtx, pFrame, &frameFinished, &packet);
      
      //On successful decode, convert the image from its native format to RGB
      if(frameFinished) {
	pFrameRGB = scaleFrame(pFrame, pCodecCtx->pix_fmt);	
      }   
    // Free the packet that was allocated by av_read_frame
    av_free_packet(&packet);
  }

  return pFrameRGB;
}

/*
 * Encode an AVFrame into an AVPacket, using the COOL encoder
 * This code is based on ffmpeg doc/examples/decoding_encoding.c
 */
static void encode(AVPacket& encPkt, AVFrame* frame)
{
  AVCodecContext* outCtx = NULL;
  AVCodec* outCodec = NULL;
  int gotPacket;
  outCodec = avcodec_find_encoder(AV_CODEC_ID_COOL);

  //Initialize the packet
  encPkt.data = NULL;
  encPkt.size = 0;
  av_init_packet(&encPkt);

  //Set up the CodecContext
  outCtx = avcodec_alloc_context3(outCodec);
  outCtx->time_base=(AVRational){1,25};
  outCtx->pix_fmt = outCodec->pix_fmts[0];
  outCtx->width = frame->width;
  outCtx->height = frame->height;

  //Open the cool codec
  if(avcodec_open2(outCtx, outCodec, NULL) < 0){
    cout << "Could not open codec" << endl;
    exit(1);
  }

  //Scale the frame if the codec doesn't use RGB24
  if(outCodec->pix_fmts[0] != AV_PIX_FMT_RGB24){
    frame = scaleFrame(frame, outCodec->pix_fmts[0]);
  }

  //Encode the frame into the packet
  avcodec_encode_video2(outCtx, &encPkt, frame, &gotPacket);
}

static void write_frame(AVPacket *encoded_frame, int frame_num) {
  FILE* file;
  char filename[15];
  sprintf(filename, "frame%03d.cool", frame_num);
  file = fopen(filename, "wb");
  fwrite(encoded_frame->data, 1, encoded_frame->size, file);
}

/*
 * Sets pixel values in an AVFrame based on the x and y coordinates passed in
 * Works for RGB24 format - sets values for r, g, and b
 */
static void setPixel(AVFrame* frame, int x, int y, int r, int g, int b)
{
  int offset = (3*x + y*frame->linesize[0]);
  frame->data[0][offset] = r;
  frame->data[0][offset+1] = g;
  frame->data[0][offset+2] = b;
}

/*
 * Draw a sphere over a background image and return the result.
 * TODO clean up drawing logic 
 */
static AVFrame* draw_ball(AVFrame *bg_img, int mid_x, int mid_y, int radius){
  // copy the bakground image into a new image
  AVFrame *write_frame = scaleFrame(bg_img, AV_PIX_FMT_RGB24);

  // highlight circle information
  // drawn with a quarter radius on the upper right part of the circle
  int hl_radius = radius/4;
  int hl_y = mid_y - radius*2/3;
  int hl_x = mid_x + radius/2;
  int hl2_radius = hl_radius*3/2; // darker highlight for 'fade'

  // define a square around the circle
  int left_lim = mid_x - radius;
  int right_lim = mid_x + radius;
  int top_lim = mid_y - radius;
  int bot_lim = mid_y + radius;

  // in the square: color each point if it is within the radius
  for(int y = top_lim; y < bot_lim; y++){
    for(int x = left_lim; x < right_lim; x++){
      // avoid square roots for efficiency
      int dist_center_sqr = (x-mid_x)*(x-mid_x) + (y-mid_y)*(y-mid_y);
      int dist_hl_sqr = (x-hl_x)*(x-hl_x) + (y-hl_y)*(y-hl_y);

      // draw if within circle radius (pythagorean threorm)
      if(dist_center_sqr <= radius*radius){
        
        // determine color based off highlighting
        int color = 0; // default (black)
        if(dist_hl_sqr <= hl_radius*hl_radius){
	  color = 80;
	}
        else if(dist_hl_sqr <= hl2_radius*hl2_radius){
	  color = 70;
	}

	setPixel(write_frame, x, y, color, color, color);
      }
    }
  }

  return write_frame;
}

/*
 * Calculate the next coordinates for the ball.
 */
static void move_ball(int image_height, int falling_speed, int &x, int &y, int &radius, bool &falling) {
    int bottom_edge = y + radius;
    int top_edge = y - radius;

    // switch direction if needed
    if (bottom_edge >= image_height) {
        falling = false;
        // note: repositioning to avoid trying to draw the ball off the
        //       image in the next frame, giving segfault
	y = image_height - radius - falling_speed;
    }
    else if (top_edge <= 0) {
        falling = true;
	y = radius + falling_speed;
    }

    // move forward
    if (falling) {
        y += falling_speed;
    }
    else {
        y -= falling_speed;
    }
}


/*
 * Check that the extension on a filename is 'jpeg' or 'jpg'.
 *
 * Return true if the extension is 'jpeg' or 'jpg' (case sensitive), or
 * false otherwise.
 */
static bool check_image_extension(string filename) {
  int dot_idx = filename.find(".");
  string ext = filename.substr(dot_idx+1);

  if (ext != "jpeg" && ext != "jpg") {
    return false;
  }
  return true;
}


int main(int argc, char *argv[]) {
  const int frames = 300; // num of frames to generate

  if(argc < 2) {
    cerr << "An image filename must be given as the first argument" << endl;
    return -1;
  }
  if (!check_image_extension(argv[1])) {
    cerr << "Image filename must have a 'jpeg' or 'jpg' extension" << endl;
    return -1;
  }

  // Open the file and decode it into an RGB24 frame
  AVFrame* background_image = NULL;
  background_image = makeDecodedFrame(argv[1]);

  // create and write each frame
  int ball_x = background_image->width/2; // initially at middle of img
  int ball_y = background_image->height/2;
  int ball_rad = background_image->width/10;  // 10% of img width
  AVPacket encoded_frame; // local to avoid allocation issues
  bool falling = true;
  int falling_speed = 10;
  int bg_img_height = background_image->height;
  for (int frame_num = 0; frame_num < frames; frame_num++) {
    // draw ball -> encode frame -> write frame -> move ball
    AVFrame *drawn_frame = draw_ball(background_image, ball_x, ball_y, ball_rad);
    encode(encoded_frame, drawn_frame);
    write_frame(&encoded_frame, frame_num);
    move_ball(bg_img_height, falling_speed, ball_x, ball_y, ball_rad, falling);
  }

  // Create, write 300 animation frames using the decoded frame as background
  //makeFrames(300, pFrameRGB);
 
  return 0;
}
