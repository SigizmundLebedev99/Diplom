<template>
    <v-bottom-sheet v-model="opened" persistent>
        <v-toolbar dense class="primary" dark>
            <v-toolbar-title class="title">Добавление файлов</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-btn icon @click="close()">
                <v-icon>close</v-icon>
            </v-btn>
        </v-toolbar>
        <div class="white">
            <v-layout column class="mr-3 ml-3 block">
                <v-layout row align-center wrap>
                    <span class="subheading mt-1">Прикрепите имеющиеся в проекте файлы или </span>
                    <span class="subheading loader mt-1 ml-1" @click="onFocus()">загрузите новые</span>
                    <input type="file" v-show="false"
                        :multiple="false" 
                        ref="fileInput" 
                        @change="onFileChange">   
                    <v-spacer/>
                    <div class="constraint">
                        <v-text-field prepend-icon="search" v-model="filterText"></v-text-field>
                    </div>
                </v-layout>
                <v-divider></v-divider>
                <v-layout row wrap>
                    <v-flex md1></v-flex>
                    <v-flex md4 xs12>
                        <v-layout column>
                            <p class="text-xs-center mb-0 mt-1">Прикрепленные файлы</p>
                            <v-layout row wrap class="mb-2 mt-2" justify-center max-width="400px">
                                <v-chip label v-for="(f,i) in selFiles" :key="i" class="primary" dark @click="removeFile(f.id)">
                                    <v-icon small>remove</v-icon>
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
                        </v-layout>
                    </v-flex>
                    <v-flex md1></v-flex>
                    <v-divider vertical></v-divider>
                    <v-flex md1></v-flex>
                    <v-flex md4 xs12>
                        <v-layout column>
                            <p class="text-xs-center mb-0 mt-1">Файлы проекта</p>
                            <v-container v-show="loading">
                                <v-layout column justify-center align-center fill-height>
                                    <v-progress-circular indeterminate color="primary"></v-progress-circular>
                                </v-layout>
                            </v-container>
                            
                            <v-layout row wrap v-show="!loading" class="mb-2 mt-2" justify-center max-width="400px">
                                <v-chip label v-for="(f,i) in filteredFiles" :key="i" class="primary" dark @click="addFile(f)">
                                    <v-icon small>add</v-icon>
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
                        </v-layout>
                    </v-flex>
                    <v-flex md1></v-flex>
                </v-layout>
            </v-layout>
        </div>
    </v-bottom-sheet>
</template>

<script>
import {mapGetters, mapMutations, mapActions} from 'vuex'
export default {
    data:()=>({
        filterText:''
    }),
    methods:{
        ...mapMutations({
            addFile:'fileSelector/addFile',
            removeFile:'fileSelector/removeFile'
        }),
        ...mapActions({close:'fileSelector/close'}),
        onFocus(event){
            this.$refs.fileInput.click();
        },
        onFileChange($event){
            const files = $event.target.files || $event.dataTransfer.files;
            if (!files.length) 
                return;
            var fd  = new FormData();
            fd.append('file', files[0]);
            this.$http.post(`/api/file/project/${this.$route.params.projId}`, fd, { headers: {'Content-Type': 'multipart/form-data' }}).then(
                r=>{this.addFile(r.data)},
                r=>{console.log(r.responce);}
            );
        }
    },
    computed:{
        ...mapGetters({
            opened:'fileSelector/opened',
            project:'project/project',
            loading:'fileSelector/loading',
            files:'fileSelector/files',
            selFiles:'fileSelector/selectedFiles'
        }),
        filteredFiles(){
            if(this.filterText && this.files)
                return this.files.filter(e=>e.fileName.toLowerCase().startsWith(this.filterText.toLowerCase()));
            else
                return this.files;
        }
    }
}
</script>

<style scoped>
.constraint{
    width: 200px;
}

.block{
    min-width:200px;
}

.loader{
    cursor: pointer;
    color: blue;
    text-decoration: underline;
}
</style>

