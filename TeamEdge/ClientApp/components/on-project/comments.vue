<template>
    <div>
        <v-layout justify-space-between>
            <v-btn class="grey text-none white--text" small flat @click="preCommentCreating()">
                {{formOpened?"Отмена":"Написать комментарий"}}
            </v-btn>
            <v-btn icon small @click="fetchComments()">
                <v-icon>refresh</v-icon>
            </v-btn>
        </v-layout>
        <template v-if="formOpened">
            <v-textarea class="mx-2 mt-2 mb-0"
                hide-details
                outline
                name="input-7-4"
                label="Ваш комментарий" v-model="text"></v-textarea>
            <v-layout row wrap v-show="!loading" class="mb-2 mt-2" justify-center max-width="400px">
                <v-chip label v-for="(f,i) in files" :key="i" class="primary" dark>
                    <div class="mr-1 ml-1">
                        <v-icon v-if="f.isPicture">
                            image
                        </v-icon>
                        <v-icon v-else>
                            insert_drive_file
                        </v-icon>
                    </div>
                    <span>{{f.fileName}}</span>
                </v-chip>
            </v-layout>
            <v-layout justify-end>
                <v-btn icon small @click="openFileSelector()">
                    <v-icon>attach_file</v-icon>
                </v-btn>
                <v-btn class="primary" @click="send()" small>OK</v-btn>
            </v-layout>
        </template>
        <v-divider class="grey"></v-divider>
        <v-layout justify-center v-show="loading" class="mt-4">
            <v-progress-circular indeterminate color="primary"></v-progress-circular>
        </v-layout>
        <div v-show="!loading">
            <p class="text-xs-center subheading mt-2" v-if="!comments || !comments.length">Пока нет комментариев</p>
            <v-list  class="transparent">
                <v-list-tile v-for="(c,i) in comments" :key="i" class="mb-2">
                    <v-list-tile-avatar>
                        <v-avatar color="primary" size="30">
                            <span v-if="!c.user.avatar" class="white--text text-xs-center" medium>
                                {{`${c.user.fullName.split(' ').map(s=>s[0]).join('')}`}}
                            </span>
                            <v-img v-else :src="c.user.avatar"/>
                        </v-avatar>
                    </v-list-tile-avatar>
                    <div class="comment px-2 py-1 ml-0">
                        <span class="comment-text">{{c.text}}</span>
                    </div>
                </v-list-tile>
            </v-list>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapActions} from 'vuex'

export default {
    mounted() {
        this.fetchComments();
    },
    data:()=>({
        text:'',
        loading: false,
        comments: [],
        files:[],
        formOpened:false
    }),
    methods:{
        fetchComments(){
            this.loading = true;
            this.$http.get(`/api/comment/workitem/${this.currentWI.descriptionId}`)
            .then(
                r=>{
                    this.comments = r.data;
                    this.loading = false;
                },
                r=>console.log(r.response)
            );
        },
        ...mapActions({
            open:'fileSelector/open'
        }),
        openFileSelector(){
            this.open({  
                selectedFiles: this.files,
                onClose:selFiles => this.setFiles(selFiles)
            })
        },
        setFiles(files){
            this.files = files;
        },
        preCommentCreating(){
            if(this.formOpened)
            {
                this.formOpened = false;
                this.text = "";
                this.files = [];
            }
            else{
                this.formOpened = true;
            }
        },
        send(){
            var model = {
                text:this.text,
                workItemId:this.currentWI.descriptionId,
                files:this.files.map(e=>e.id)
            }
            this.$http.post(`/api/comment`,model).then(
                r=>{this.fetchComments();this.preCommentCreating();},
                r=>{console.log(r.response)}
            )
        }
    },
    computed:{
        ...mapGetters({currentWI:'project/currentWI'})
    },
    watch:{
        currentWI(from, to){
            this.fetchComments();
        }
    }
}
</script>

<style scoped>
.comment{
    width:100%;
    border-left: 3px solid grey;
}

.comment-text{
    font-size: 12px;
}
</style>
