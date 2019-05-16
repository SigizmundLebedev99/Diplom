<template>
    <v-layout>
        <v-flex md6 sm8 xs10 offset-md3 offset-xs1 offset-sm2>
            <v-layout justify-space-between v-show="!formOpened">
                <v-btn class="grey text-none white--text ml-0" small flat @click="preCommentCreating()">
                    Написать комментарий
                </v-btn>
                <v-btn icon small @click="fetchComments()" class="mr-0">
                    <v-icon>refresh</v-icon>
                </v-btn>
            </v-layout>
            <template v-if="formOpened">
                <v-textarea class="mx-2 mt-2 mb-0"
                    hide-details
                    outline
                    name="input-7-4"
                    label="Ваш комментарий" v-model="text"></v-textarea>
                <v-layout row wrap v-show="!loading" class="my-2" justify-center max-width="400px">
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
                    <v-btn class="primary text-none" @click="send()" small>Отправить</v-btn>
                    <v-btn class="grey text-none white--text" small flat @click="preCommentCreating()" v-show="formOpened">
                        Отмена
                    </v-btn>
                </v-layout>
            </template>
            <v-divider class="grey"></v-divider>
            <v-layout justify-center v-show="loading" class="my-4">
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
            <div v-show="!loading">
                <p class="text-xs-center mt-4" v-if="!comments || !comments.length">Пока нет комментариев</p>
                <v-layout v-for="(c,i) in comments" :key="i" class="my-3 mr-2">
                    <v-avatar color="primary" size="30">
                        <span v-if="!c.user.avatar" class="white--text text-xs-center" medium>
                            {{`${c.user.name.split(' ').map(s=>s[0]).join('')}`}}
                        </span>
                        <v-img v-else :src="c.user.avatar"/>
                    </v-avatar>
                    <v-layout column>
                        <div class="comment px-2 py-1 ml-2">
                            <span class="comment-text">{{c.text}}</span>
                            <v-layout row wrap align-start justify-center>
                                <v-card v-for="(f,i) in c.files" :key="i" class="mx-1 my-1">
                                    <v-card-text class="py-1 px-1">
                                        <v-layout justify-center align-center fill-height>
                                        <img height="128px" v-if="f.isPicture" :src="f.path"/>
                                        <v-icon v-else>
                                            insert_drive_file
                                        </v-icon>
                                        </v-layout>
                                    </v-card-text>
                                    <v-card-actions>
                                        <span class="caption">{{f.name}}</span>
                                    </v-card-actions>
                                </v-card>
                            </v-layout> 
                        </div>
                        <v-layout justify-end>
                            <span class="date-text">{{getDate(c.dateOfCreation)}}</span>
                        </v-layout>
                    </v-layout>
                </v-layout>
            </div>
        </v-flex>
    </v-layout>
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
                r=>{
                    this.files.forEach(f=>this.currentWI.changed.description.files.push(f));
                    this.fetchComments();
                    this.preCommentCreating();
                },
                r=>{console.log(r.response)}
            )
        }
    },
    computed:{
        ...mapGetters({currentWI:'project/currentWI'})
    },
    watch:{
        currentWI(){
            this.fetchComments();
        }
    }
}
</script>

<style scoped>
.comment{
    width:100%;
    border-left: 3px solid grey;
    background-color: #dad6d6;
}

.comment-text{
    font-size: 12px;
}
</style>
