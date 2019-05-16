<template>
    <div v-if="currentWI">
        <v-container v-show="loading">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-show="!loading">
            <v-layout class="mr-5" justify-end>
                <span class="my-2 mr-5 subheading" v-if="!localFiles.length">Пока нет вложений</span>
                <v-btn small icon @click="openFileSelector()">
                    <v-icon>
                        attach_file
                    </v-icon>
                </v-btn>
                <v-btn small icon @click="fetchFiles">
                    <v-icon>
                        refresh
                    </v-icon>
                </v-btn>
            </v-layout>
            <v-layout row wrap align-start justify-center>
                <v-card v-for="(f,i) in localFiles" :key="i" class="mx-1 my-1">
                    <v-card-text class="px-1 py-1">
                        <v-layout justify-center align-center fill-height>
                        <img height="128px" v-if="f.isPicture" :src="f.path"/>
                        <v-icon v-else class="mt-2">
                            insert_drive_file
                        </v-icon>
                        </v-layout>
                    </v-card-text>
                    <v-card-actions>
                        <span class="caption">{{f.fileName}}</span>
                    </v-card-actions>
                </v-card>
            </v-layout>
        </div>
    </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
export default {
    mounted(){
        this.fetchFiles();
    },
    data:()=>({
        loading:false,
        localFiles:[],
    }),
    computed:{
        ...mapGetters({currentWI:'project/currentWI'})
    },
    methods:{
        fetchFiles(){
            this.loading = true;
            this.$http.get(`/api/project/${this.$route.params.projId}/item/${this.currentWI.descriptionId}/files`)
            .then(
                r=>{
                    this.currentWI.source.description.files = r.data;
                    this.currentWI.changed.description.files = r.data;
                    this.localFiles = r.data
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
                selectedFiles: this.currentWI.changed.description.files,
                onClose:selFiles => this.setFiles(selFiles)
            })
        },
        setFiles(files){
            this.currentWI.changed.description.files = files;
            this.localFiles = files;
        }
    },
    watch:{
        currentWI(to, from){
            if(!to)
                return;
            if(!to.changed.description.files || !to.changed.description.files.length){
                this.fetchFiles();
            }
            else{
                this.localFiles = to.changed.description.files;
            }
        }
    }
}
</script>
