import { TextGenerateEffect } from "../ui/text-generate-effect";
import { Vortex } from "../ui/vortex";
import { motion } from "framer-motion";
import Autoplay from "embla-carousel-autoplay";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";

export function Hero() {
  return (
    <Carousel
      opts={{ loop: true }}
      plugins={[
        Autoplay({
          delay: 5000,
        }),
      ]}
      className="bg-black"
    >
      <CarouselContent>
        <CarouselItem>
          <div className="w-screen mx-auto rounded-md h-[600px] overflow-hidden">
            <Vortex
              backgroundColor="black"
              rangeY={800}
              particleCount={500}
              className="flex items-center flex-col justify-center px-2 md:px-10  py-4 w-full h-full gap-5"
            >
              <motion.h2
                className="text-secondary text-6xl md:text-8xl font-bold text-center"
                initial={{ opacity: 0, filter: "blur(10px)" }}
                animate={{ opacity: 1, filter: "blur(0px)" }}
                transition={{ duration: 1.5 }}
              >
                FulFlix
              </motion.h2>
              <p className="text-white text-sm md:text-3xl max-w-xl mt-6 text-center">
                Where happiness is fulfilled
              </p>
            </Vortex>
          </div>
        </CarouselItem>
        <CarouselItem>
          <div className="w-screen mx-auto rounded-md h-[600px] overflow-hidden bg-black">
            <h2 className="text-secondary text-4xl md:text-8xl font-bold text-center justify-start items-center">
              Title
            </h2>
            <p className="text-white text-sm md:text-3xl max-w-xl mt-6 text-center">
              Book ticket
            </p>
          </div>
        </CarouselItem>
      </CarouselContent>
      <CarouselPrevious className="hidden md:block" />
      <CarouselNext className="hidden md:block" />
    </Carousel>
  );
}
