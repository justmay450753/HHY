<template>
    <div class="col-md-6 admincontent">
        <div class="form-group admin_content_part">
            <div class="col-md-6">
                <label class="control-label">標題</label>
                <input class="form-control" v-model="edit.title" />
            </div>
            <div class="col-md-6">
                <label class="control-label">連結</label>
                <input class="form-control" v-model="edit.url" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-12">
                <label class="control-label">內容</label>
                <vue-ckeditor v-model="edit.content"></vue-ckeditor>
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-5">
                <label class="control-label">上檔日期</label>
                <div class="input-group date" id="datetimepickerstart" data-target-input="nearest">
                    <input id="startDate" type="text" class="form-control datetimepicker-input" data-target="#datetimepickerstart" v-model="edit.publishDate" />
                    <div class="input-group-append" data-target="#datetimepickerstart" data-toggle="datetimepicker">
                        <div class="input-group-text"><i data-feather="calendar"></i></div>
                    </div>
                </div>
                <input type="hidden" class="hidPublishDate" />
            </div>
            <div class="col-md-5">
                <label asp-for="DownDate" class="control-label"></label>
                <div class="input-group date" id="datetimepickerend" data-target-input="nearest">
                    <input id="endDate" type="text" class="form-control datetimepicker-input" data-target="#datetimepickerend" v-model="edit.downDate" />
                    <div class="input-group-append" data-target="#datetimepickerend" data-toggle="datetimepicker">
                        <div class="input-group-text"><i data-feather="calendar"></i></div>
                    </div>
                </div>
                <input type="hidden" class="hidDownDate" />
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-12">
                <label class="control-label">圖片連結</label><br />
                <div>
                    <input type="file" v-on:change="previewImage($event, fileInfo)" accept="image/*">
                </div>
                <img style="width: 90%;margin-top: 10px;" :src="fileInfo.imageData">
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6 no_chksend">
                <input type="submit" value="儲存" class="btn btn-info" v-on:click='editBlog' />
            </div>
        </div>
    </div>
</template>

<script>
    module.exports = {
        data: function () {
            return {
                fileInfo:
                {
                    file: null,
                    imageData: ''
                }
                ,
                edit: {
                    ID: null,
                    Title: null,
                    Url: null,
                    ImgUrl: null,
                    Content: null,
                    PublishDate: null,
                    DownDate: null,
                    file: null
                }
            }
        },
        mounted: function () {
            this.getBlog();
        },
        methods: {
            getBlog: function () {
                var self = this;
                var url = window.location.search;
                var arr = url.split("id=");
                if (arr.length > 1)
                    var id = arr[1];
                $.get("/api/admin/getblog?id=" + id)
                    .done(function (result) {
                        self.edit = result;
                        $(".hidPublishDate").val(result.publishDate);
                        $(".hidDownDate").val(result.downDate);
                        self.fileInfo.imageData = result.imgUrl;
                    })
                    .fail(function (xhr) {
                        alert(xhr.responseJSON.Message);
                    });
            },
            editBlog: function () {
                var self = this;
                var formData = new FormData();
                self.edit.PublishDate = $(".hidPublishDate").val();
                self.edit.DownDate = $(".hidDownDate").val();
                formData.append('ID', self.edit.id);
                formData.append('Title', self.edit.title);
                formData.append('Url', self.edit.url);
                formData.append('file', self.fileInfo.file);
                formData.append('PublishDate', self.edit.PublishDate);
                formData.append('DownDate', self.edit.DownDate);
                formData.append('Content', self.edit.content);

                $.ajax({
                    contentType: false,
                    processData: false,
                    type: 'Put',
                    url: '/api/admin/editblog',
                    data: formData,
                    statusCode: {},
                    success: function () {
                        alert('上傳成功');
                    },
                    error: function (xhr, textStatus, error) {
                        alert(xhr.responseJSON.Message);
                    }
                });
            },
            previewImage: function (event, fileInfo) {
                var input = event.target;
                var self = this;
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {

                        fileInfo.file = input.files[0];
                        fileInfo.imageData = e.target.result;
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }
        },
        components: {
            'vue-ckeditor': httpVueLoader('/vue/VueCkeditor.vue')
        }
    }
</script>
