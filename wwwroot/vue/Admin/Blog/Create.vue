<template>
    <div class="col-md-6 admincontent">
        <div class="form-group admin_content_part">
            <div class="col-md-6">
                <label class="control-label">標題</label>
                <input class="form-control" v-model="insert.Title" />
            </div>
            <div class="col-md-6">
                <label class="control-label">連結</label>
                <input class="form-control" v-model="insert.Url" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-12">
                <label class="control-label">內容</label>
                <vue-ckeditor v-model="insert.Content"></vue-ckeditor>
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6">
                <label class="control-label">上檔日期</label>
                <div class="input-group date" id="datetimepickerstart" data-target-input="nearest">
                    <input id="startDate" type="text" class="form-control datetimepicker-input" data-target="#datetimepickerstart" v-model="insert.PublishDate" />
                    <div class="input-group-append" data-target="#datetimepickerstart" data-toggle="datetimepicker">
                        <div class="input-group-text"><i data-feather="calendar"></i></div>
                    </div>
                </div>
                <input type="hidden" class="hidPublishDate" />
            </div>
            <div class="col-md-6">
                <label class="control-label">下檔日期</label>
                <div class="input-group date" id="datetimepickerend" data-target-input="nearest">
                    <input id="endDate" type="text" class="form-control datetimepicker-input" data-target="#datetimepickerend" v-model="insert.DownDate" />
                    <div class="input-group-append" data-target="#datetimepickerend" data-toggle="datetimepicker">
                        <div class="input-group-text"><i data-feather="calendar"></i></div>
                    </div>
                </div>
                <input type="hidden" class="hidDownDate" />
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-12">
                <label class="control-label">圖片連結</label><br/>
                <div>
                    <input type="file" v-on:change="previewImage($event, fileInfo)" accept="image/*">
                </div>
                <img style="width: 90%;margin-top: 10px;" :src="fileInfo.imageData">
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6 no_chksend">
                <input type="submit" value="建立" class="btn btn-info" v-on:click='createBlog' />
            </div>
        </div>
    </div>
</template>
<script type="text/javascript">
    $(function () {
        $('#datetimepickerstart').datetimepicker({
            format: 'YYYY/MM/DD HH:mm:ss'
        });
        $('#datetimepickerend').datetimepicker({
            useCurrent: false,
            format: 'YYYY/MM/DD HH:mm:ss'
        });
        $("#datetimepickerstart").on("change.datetimepicker", function (e) {
            $(".hidPublishDate").val($(this).val());
            $('#datetimepickerend').datetimepicker('minDate', e.date);
        });
        $("#datetimepickerend").on("change.datetimepicker", function (e) {
            $(".hidDownDate").val($(this).val());
            $('#datetimepickerstart').datetimepicker('maxDate', e.date);
        });
    })
</script>
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
                insert: {
                    Title: null,
                    Url: null,
                    ImgUrl: null,
                    Content: null,
                    PublishDate: null,
                    DownDate: null
                }
            }
        },
        mounted: function () {
            
        },
        methods: {
            createBlog: function () {
                var self = this;
                var formData = new FormData();
                self.insert.PublishDate = $(".hidPublishDate").val();
                self.insert.DownDate = $(".hidDownDate").val();

                formData.append('Title', self.insert.Title);
                formData.append('Url', self.insert.Url);
                formData.append('file', self.fileInfo.file);
                formData.append('PublishDate', self.insert.PublishDate);
                formData.append('DownDate', self.insert.DownDate);
                formData.append('Content', self.insert.Content);

                $.ajax({
                    contentType: false,
                    processData: false,
                    type: 'Post',
                    url: '/api/admin/createblog',
                    data: formData,
                    statusCode: {},
                    success: function () {
                        alert('上傳成功');
                        location.href = '/Blogs';
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
