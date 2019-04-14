<template>
    <div>
        <v-container v-show="loading">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-show="!loading">
            <v-layout>
                <span class="mt-2 ml-3 subheading" v-if="!localFiles.length">Пока нет вложений</span>
                <v-spacer/>
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
            <v-layout row wrap justify-center>
                <v-card v-for="(f,i) in localFiles" :key="i" class="mx-1 my-1">
                    <v-card-text class="mx-1 my-1">
                        <img height="128px" v-if="f.isPicture" :src="f.path"/>
                        <v-icon v-else>
                            insert_drive_file
                        </v-icon>
                    </v-card-text>
                    <span class="pl-3 caption">{{f.fileName}}</span>
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
                    this.currentWI.description.files = r.data;
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
        currentWI(from, to){
            if(to.changed.description.files)
                this.localFiles = to.changed.description.files;
            else
                this.fetchFiles();
        }
    }
}
</script>
