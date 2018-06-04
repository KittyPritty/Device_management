package hello;


import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RestController;

import java.util.Arrays;
import java.util.stream.Collectors;

@RestController
public class BaseController {// базовий контроллер, який має функцію яка повертає відповідь при момилці
    protected ResponseEntity getErrorResponse(Exception e) {
        return ResponseEntity.status(500)
                .body(Arrays.stream(e.getStackTrace()).map(StackTraceElement::toString).collect(Collectors.toList()));
    }

}
