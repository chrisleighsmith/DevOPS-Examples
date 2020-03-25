import org.apache.log4j.Logger;

public class LoggingSample {
  static Logger log = Logger.getLogger(LoggingSample.class);

  public static void main(String[] args) {
   
     log.fatal("Sample FATAL message");
     log.error("Sample ERROR message");
     log.warn("Sample WARN message");
     log.info("Sample INFO message");
     log.debug("Sample DEBUG message");
     log.trace("Sample TRACE message");
  }
}