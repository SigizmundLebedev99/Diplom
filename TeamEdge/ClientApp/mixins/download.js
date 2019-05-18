export default {
    methods:{
        downloadFile(id) {
            this.$http
              .post(
                `/api/file/${id}`,
                {},
                { responseType: "arraybuffer" }
              )
              .then(
                response => {
                    var name = this.getFileNameFromHttpResponse(response);
                    let blob = new Blob([response.data], {
                        type: response.headers["content-type"]
                    });
                    let link = document.createElement("a");
                    link.href = window.URL.createObjectURL(blob);
                    link.download = name;
                    link.click();
                },
                response => {
                    console.log(response.response);
                }
            );
        },
        getFileNameFromHttpResponse(httpResponse) {
            console.log(httpResponse);
            var contentDispositionHeader = httpResponse.headers["content-disposition"];
            var result = contentDispositionHeader
                .split(";")[1]
                .trim()
                .split("=")[1];
            return result.replace(/"/g, "");
        }
    }
}